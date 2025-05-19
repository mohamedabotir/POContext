using Common.Repository;
using Infrastructure.Producers;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Serilog;

public class OutboxProcessor
{
    private readonly IEventRepository _eventRepo;
    private readonly IProducer _producer;
    private readonly string _topic;
    private readonly AsyncRetryPolicy _retryPolicy;

    public OutboxProcessor(IEventRepository eventRepo, IProducer producer, IOptions<Topic> options)
    {
        _eventRepo = eventRepo;
        _producer = producer;
        _topic = options.Value.TopicName;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Log.Warning($"🔁 Retry {retryCount} for event {context["EventId"]} after {timeSpan.TotalSeconds}s: {exception.Message}");
                });
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var unprocessed = await _eventRepo.GetUnprocessedEventsAsync();

        foreach (var evt in unprocessed)
        {
            var context = new Context();
            context["EventId"] = evt.Id.ToString();

            try
            {
                await _retryPolicy.ExecuteAsync(async ctx =>
                {
                    await _producer.ProduceAsync(_topic, evt.EventBaseData);
                    await _eventRepo.MarkAsProcessed(evt.Id);
                }, context);
            }
            catch (Exception ex)
            {
                Log.Error($"❌ Failed after 3 retries for event {evt.Id}: {ex.Message}");
            }
        }

        Log.Information("✅ Finished processing outbox events.");
    }
}
