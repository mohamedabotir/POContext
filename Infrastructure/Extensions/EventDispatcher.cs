using Domain.DomainEvents;
using Domain.Handlers;
using Infrastructure.EventHandlers;

namespace Infrastructure.Extensions;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public EventDispatcher()
    {
        
    }
    public async Task DispatchDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = (IDomainEventHandler<IDomainEvent>) _serviceProvider.GetService(handlerType)!;
            await handler.HandleAsync(domainEvent);
        }
        
    }
}