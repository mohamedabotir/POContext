using Common.Constants;
using Common.Events;
using Common.Repository;
using Common.Utils;
using Infrastructure.Exceptions;

namespace Infrastructure.Handlers;

public class EventHandler(IPurchaseOrderRepository purchaseOrderRepository,IEventDispatcherWithFactory eventDispatcher):IEventHandler
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

    public async Task On(OrderClosed @event)
    {
       await eventDispatcher.DispatchDomainEventAsync([@event]);
    }
}