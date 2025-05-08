using Common.Constants;
using Common.Entity;
using Common.Events;
using Common.Repository;
using Common.Utils;
using Domain.DomainEvents;
using System;

namespace Infrastructure.Handlers;

public class EventHandler(IEventSourcing<PoEntity> eventSourcing,IPurchaseOrderRepository purchaseOrderRepository,IEventDispatcherWithFactory eventDispatcher):IEventHandler
{
    public async Task On(OrderBeingShipped @event)
    {
            var aggregate = await eventSourcing.GetByIdAsync(@event.PoNumber);
        if(aggregate.MakeOrderBeingShipped(@event).IsFailure)
            return;
            await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid,PurchaseOrderStage.BeingShipped);
            await eventSourcing.SaveAsync(aggregate);
    }

    public async Task On(OrderShipped @event)
    {
        var aggregate = await eventSourcing.GetByIdAsync(@event.PoNumber);
        if (aggregate.MakeOrderShipped(@event).IsFailure)
            return;

        await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid,PurchaseOrderStage.Shipped);
        await eventSourcing.SaveAsync(aggregate);

    }

    public async Task On(OrderClosed @event)
    {
        var aggregate = await eventSourcing.GetByIdAsync(@event.PoNumber);
        if (aggregate.ClosePurchaseOrder(@event).IsFailure)
            return;
        await purchaseOrderRepository.UpdatePoStageWithFactoryAsync(@event.PurchaseOrderGuid, PurchaseOrderStage.Closed);
        await eventSourcing.SaveAsync(aggregate);
    }
}