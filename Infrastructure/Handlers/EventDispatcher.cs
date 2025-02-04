using Common.Events;
using Common.Handlers;

namespace Infrastructure.Handlers;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchDomainEventsAsync(IEnumerable<DomainEventBase> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = serviceProvider.GetService(handlerType);
            if (handler != null)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                await ((Task)handleMethod?.Invoke(handler, [domainEvent ,CancellationToken.None])!);
            }
        }
    }

}