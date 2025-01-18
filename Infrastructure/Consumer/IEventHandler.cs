using Common.DomainEvents;
using Common.Events;

namespace Infrastructure.Consumer;

public interface IEventHandler
{
    public Task On(OrderBeingShipped @event);
}