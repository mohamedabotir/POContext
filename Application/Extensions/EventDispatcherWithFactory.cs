using Common.Events;
using Common.Handlers;
using Common.Utils;

namespace Application.Extensions;

public class EventDispatcherWithFactory:IEventDispatcherWithFactory
{
    private readonly IServiceProviderFactory _serviceProviderFactory;
    public EventDispatcherWithFactory(IServiceProviderFactory serviceProviderFactory)
    {
        _serviceProviderFactory = serviceProviderFactory;
    }

    public async Task DispatchDomainEventAsync(IEnumerable<DomainEventBase> domainEvents)
    {
        var provider = _serviceProviderFactory.CreateScope();
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = provider.GetService(handlerType);
            if (handler != null)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                await (Task)handleMethod.Invoke(handler, new object[] { domainEvent ,CancellationToken.None});
            }
        }
        
    }
}