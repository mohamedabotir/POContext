using Domain.DomainEvents;

namespace Domain.Handlers;

public interface IEventDispatcher
{
    Task DispatchDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents);
}