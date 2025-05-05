using Common.Repository;
using Infrastructure.Producers;
using Microsoft.Extensions.Options;

public class OutboxProcessor
{
    private readonly IEventRepository _eventRepo;
    private readonly IProducer _producer;
    private readonly string _topic;

    public OutboxProcessor(IEventRepository eventRepo, IProducer producer, IOptions<Topic> options)
    {
        _eventRepo = eventRepo;
        _producer = producer;
        _topic = options.Value.TopicName;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var unprocessed = await _eventRepo.GetUnprocessedEventsAsync();
        foreach (var evt in unprocessed)
        {
            try
            {
                await _producer.ProduceAsync(_topic, evt.EventBaseData);
                await _eventRepo.MarkAsProcessed(evt.Id); // Make sure EventBaseData.Id exists
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to process event {evt.Id}: {ex.Message}");
            }
        }

        Console.WriteLine("✅ Finished processing outbox events.");
    }
}
