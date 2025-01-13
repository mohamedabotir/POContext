using Infrastructure.Repository;
using Common.DomainEvents;
using Common.Repository;

namespace Infrastructure.Consumer;

public class EventHandler(IPurchaseOrderRepository purchaseOrderRepository):IEventHandler
{
    public async Task On(PoCreatedEventBase @event)
    { 
        await purchaseOrderRepository.MarkPoAsShippedAsync(@event.PoGuid);
    }
}