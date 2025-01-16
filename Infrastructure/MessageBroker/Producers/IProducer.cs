using Common.Events;
using Common.DomainEvents;

namespace Common.Mongo.Producers;

public interface IProducer
{
    Task ProduceAsync<T>(string topic , T @event) where T :DomainEventBase ;

}