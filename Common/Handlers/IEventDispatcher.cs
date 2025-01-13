using Common.Domains;

namespace Common.Handlers;

public interface IEventDispatcher
{
    Task DispatchDomainEventsAsync(IEnumerable<DomainEventBase> domainEvents);
}