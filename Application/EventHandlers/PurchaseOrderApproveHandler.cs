using Common.DomainEvents;
using Common.Events;
using Common.Handlers;

namespace Application.EventHandlers;

public class PurchaseOrderApproveHandler(IEventStore eventStore):IEventHandler<PurchaseOrderApproved>
{
    public async Task HandleAsync(PurchaseOrderApproved @event, CancellationToken cancellationToken = default)
    {
        await  eventStore.SaveEventAsync(@event.PurchaseOrderId,@event);
    }

   
}