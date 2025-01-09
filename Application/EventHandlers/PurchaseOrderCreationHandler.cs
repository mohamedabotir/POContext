using Domain.DomainEvents;

namespace Application.EventHandlers;

public class PurchaseOrderCreationHandler(IEventStore eventStore):IDomainEventHandler<PoCreatedEventBase>
{
    public async Task HandleAsync(PoCreatedEventBase eventBase, CancellationToken cancellationToken = default)
    {
      await  eventStore.SaveEventAsync(eventBase.PoGuid,eventBase);
    }
}