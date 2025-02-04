using Application.UseCases;
using Common.Events;
using Common.Handlers;

namespace Application.Handlers;

public class PurchaseOrderClosedHandler(IPurchaseOrderClosed purchaseOrderClosed):IEventHandler<OrderClosed>
{
    public async Task HandleAsync(OrderClosed @event, CancellationToken cancellationToken = default)
    {
      await  purchaseOrderClosed.ClosePurchaseOrder(@event);
    }
}