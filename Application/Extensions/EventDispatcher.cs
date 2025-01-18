using Common.DomainEvents;
using Application.EventHandlers;
using Common.Events;
using Common.Handlers;
using Common.Utils;

namespace Application.Extensions;

public class EventDispatcher :IEventDispatcher
{
    private  IServiceProvider _serviceProvider;

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
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = _serviceProvider.GetService(handlerType);
            if (handler != null)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                await (Task)handleMethod.Invoke(handler, new object[] { domainEvent ,CancellationToken.None});
            }
        }
    }

}