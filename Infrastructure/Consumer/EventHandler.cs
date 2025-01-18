using Common.Constants;
using Infrastructure.Repository;
using Common.DomainEvents;
using Common.Events;
using Common.Repository;

namespace Infrastructure.Consumer;

public class EventHandler(IPurchaseOrderRepository purchaseOrderRepository ):IEventHandler
{
    public async Task On(OrderBeingShipped @event)
    {
            await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid,PurchaseOrderStage.BeingShipped);
    }
}