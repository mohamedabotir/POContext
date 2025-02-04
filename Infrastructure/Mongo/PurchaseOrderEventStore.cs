using Common.Entity;
using Common.Events;
using Common.Repository;
using Common.Result;
using Infrastructure.Producers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Mongo;

public class PurchaseOrderEventStore(IEventRepository eventRepository, IProducer producer, IOptions<Topic> options) : IEventStore
{

    public async Task SaveEventAsync(Guid aggregateId, DomainEventBase eventBase,List<Maybe<string>> anotherTopic)
    {
 
            var eventModel = new EventModel
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
}