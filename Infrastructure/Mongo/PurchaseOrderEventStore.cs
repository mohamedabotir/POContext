using Common.Domains;
using Confluent.Kafka;
using Domain.DomainEvents;
using Domain.Entity;
using Domain.Mongo.Producers;
using Domain.Repository;
using Microsoft.Extensions.Options;

namespace Domain.Mongo;

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