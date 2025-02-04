using Common.Events;
using Common.Handlers;
using Domain.DomainEvents;

namespace Application.Handlers;

public class PurchaseOrderCreationHandler(IEventStore eventStore):IEventHandler<PoCreatedEventBase>
{
    public async Task HandleAsync(PoCreatedEventBase eventBase, CancellationToken cancellationToken = default)
    {
      await  eventStore.SaveEventAsync(eventBase.PoGuid,eventBase);
    }
}