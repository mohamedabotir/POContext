using Domain.DomainEvents;

namespace Infrastructure.EventHandlers;

public class PurchaseOrderCreationHandler(IEventStore eventStore):IDomainEventHandler<PoCreatedEventBase>
{
    public async Task HandleAsync(PoCreatedEventBase eventBase, CancellationToken cancellationToken = default)
    {
      await  eventStore.SaveEventAsync(eventBase.PoGuid,eventBase);
    }
}