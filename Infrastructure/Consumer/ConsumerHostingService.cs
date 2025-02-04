using Infrastructure.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Consumer;

public class ConsumerHostingService(ILogger<ConsumerHostingService> logger,IServiceProvider serviceProvider,IOptions<Topic> topic):IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Event Consumer Event Running.");

        using (var scoped = serviceProvider.CreateScope())
        {
            var consumer = scoped.ServiceProvider.GetRequiredService<IEventConsumer<EventConsumer>>();
            Task.Run(() => consumer.Consume(topic.Value.TopicName), cancellationToken);

        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}