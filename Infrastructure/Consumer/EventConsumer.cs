using System.Text.Json;
using Common.Events;
using Confluent.Kafka;
using Domain.DomainEvents;
using Infrastructure.EventsSerializer;
using Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Consumer;

     public sealed class EventConsumer(
         IOptions<ConsumerConfig> consumerConfig,
         IEventHandler eventHandler,
         IServiceScopeFactory serviceProvider)
         : IEventConsumer<EventConsumer>
     {
        private ConsumerConfig ConsumerConfig { get; } = consumerConfig.Value;
        private IEventHandler EventHandler { get;  } = eventHandler;
        private IServiceScopeFactory ServiceProvider { get; } = serviceProvider;

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(ConsumerConfig)
                            .SetKeyDeserializer(Deserializers.Utf8)
                            .SetValueDeserializer(Deserializers.Utf8)
                            .Build();
            consumer.Subscribe(topic);
            while (true)
            {
                ConsumeResult<string, string>? consumeResult;

                try
                {

                    consumeResult = consumer.Consume();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("failed :{0}", ex.Message);
                    throw;
                }
               
                if (consumeResult?.Message == null) continue;
                try
                {
                    using var scope = ServiceProvider.CreateScope();
                    using var sc = scope.ServiceProvider.CreateScope();
                    var options = new JsonSerializerOptions() { Converters = { new EventJsonConverter() } };
                    var @event = JsonSerializer.Deserialize<DomainEventBase>(consumeResult.Message.Value, options);
                    var handlers = EventHandler.GetType().GetMethod("On", [@event!.GetType()]);

                    if (handlers == null)
                        throw new ArgumentNullException($"Handler Didn't support method with type : {@event.GetType().Name} ");
                    handlers.Invoke(EventHandler, [@event]);
                    consumer.Commit(consumeResult);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
