using Common.Domains;
using Domain.DomainEvents;

namespace Domain.Mongo.Producers;

public interface IProducer
{
    Task ProduceAsync<T>(string topic , T @event) where T :DomainEventBase ;

}