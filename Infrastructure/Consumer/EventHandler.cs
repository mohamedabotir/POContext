using Common.Constants;
using Infrastructure.Repository;
using Common.DomainEvents;
using Common.Events;
using Common.Repository;
using Infrastructure.Exceptions;

namespace Infrastructure.Consumer;

public class EventHandler(IPurchaseOrderRepository purchaseOrderRepository ):IEventHandler
{
    public async Task On(OrderBeingShipped @event)
    {
            await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid,PurchaseOrderStage.BeingShipped);
    }

    public async Task On(OrderShipped @event)
    {
        var purchaseOrder =await purchaseOrderRepository.GetPoByPurchaseNumberWithFactoryAsync(@event.PoNumber);
        if(purchaseOrder.Value.PurchaseOrderStage != PurchaseOrderStage.BeingShipped)
           throw new CommulativeException($"Shipping order {@event.PoNumber} is not being shipped");
        await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid,PurchaseOrderStage.Shipped);
    }
}