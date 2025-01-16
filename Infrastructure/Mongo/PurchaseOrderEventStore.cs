using Common.Events;
using Common.Entity;
using Common.Mongo.Producers;
using Common.Repository;
using Confluent.Kafka;
using Common.DomainEvents;
using Microsoft.Extensions.Options;

namespace Common.Mongo;

public class PurchaseOrderEventStore(IEventRepository eventRepository, IProducer producer, IOptions<Topic> options) : IEventStore
{

    public async Task SaveEventAsync(Guid aggregateId, DomainEventBase eventBase)
    {
 
            var eventModel = new Application.Mongo.EventModel
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                AggregateType = nameof(PoEntity),
                AggregateIdentifier = aggregateId,
                EventBaseData = eventBase,
                EventType = eventBase.GetType().Name
            };
            await eventRepository.SaveEventAsync(eventModel);

            var topicName = options.Value.TopicName;
            await producer.ProduceAsync(topicName, eventBase);
 
    }

    public Task<List<DomainEventBase>> GetEventsAsync(Guid aggregateId, string collectionName = "")
    {
        throw new NotImplementedException();
    }

}