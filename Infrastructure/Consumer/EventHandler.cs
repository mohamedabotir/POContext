using Infrastructure.Repository;
using Domain.DomainEvents;
using Domain.Repository;

namespace Infrastructure.Consumer;

public class EventHandler(IPurchaseOrderRepository purchaseOrderRepository):IEventHandler
{
    public async Task On(PoCreatedEventBase @event)
    { 
        await purchaseOrderRepository.MarkPoAsShippedAsync(@event.PoGuid);
    }
}