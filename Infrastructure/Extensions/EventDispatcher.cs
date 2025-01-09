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
    public async Task DispatchDomainEventsAsync(IEnumerable<DomainEventBase> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = _serviceProvider.GetService(handlerType);
            if (handler != null)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                await (Task)handleMethod.Invoke(handler, new object[] { domainEvent ,CancellationToken.None});
            }
        }
    }
}