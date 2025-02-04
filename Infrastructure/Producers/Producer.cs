using System.Text.Json;
using Common.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Producers;

public class Producer(IOptions<ProducerConfig> options) : IProducer
{
    public ProducerConfig Config { get; set; } = options.Value;

    public async Task ProduceAsync<T>(string topic, T @event) where T : DomainEventBase
    {
            using var producer = new ProducerBuilder<string, string>(Config)
                .SetValueSerializer(Serializers.Utf8)
                .SetKeySerializer(Serializers.Utf8)
                .Build();
            Message<string, string> deliverMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };
            var result = await producer.ProduceAsync(topic, deliverMessage);
            if (result.Status == PersistenceStatus.NotPersisted)
                throw new Exception($"Cannot produce {@event.GetType()} to topic {topic} due to {result.Message}");
            
    }
}
