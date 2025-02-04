using Common.Events;

namespace Infrastructure.Producers;

public interface IProducer
{
    Task ProduceAsync<T>(string topic , T @event) where T :DomainEventBase ;

}