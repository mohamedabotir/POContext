using Domain.DomainEvents;

namespace Infrastructure.EventHandlers;

public class PurchaseOrderCreationHandler:IDomainEventHandler<PoCreatedEvent>
{
    public Task HandleAsync(PoCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        //publish event to kafka and log this event on elk
        throw new NotImplementedException();
    }
}