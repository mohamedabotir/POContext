using Common.Events;
using Common.Handlers;
using Common.Utils;

namespace Infrastructure.Handlers;

public class EventDispatcherWithFactory(IServiceProviderFactory serviceProviderFactory) : IEventDispatcherWithFactory
{
    public async Task DispatchDomainEventAsync(IEnumerable<DomainEventBase> domainEvents)
    {
        var provider = serviceProviderFactory.CreateScope();
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = provider.GetService(handlerType);
            if (handler != null)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                await (Task)handleMethod?.Invoke(handler, [domainEvent ,CancellationToken.None])!;
            }
        }
        
    }
}