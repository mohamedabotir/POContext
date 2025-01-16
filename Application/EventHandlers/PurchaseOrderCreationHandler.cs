using Common.Events;
using Common.DomainEvents;
using Common.Handlers;

namespace Application.EventHandlers;

public class PurchaseOrderCreationHandler(IEventStore eventStore):IEventHandler<PoCreatedEventBase>
{
    public async Task HandleAsync(PoCreatedEventBase eventBase, CancellationToken cancellationToken = default)
    {
      await  eventStore.SaveEventAsync(eventBase.PoGuid,eventBase);
    }
}