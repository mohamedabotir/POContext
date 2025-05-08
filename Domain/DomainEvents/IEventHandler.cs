using Common.Events;

namespace Domain.DomainEvents;

public interface IEventHandler
{
    public Task On(OrderBeingShipped @event);
    public Task On(OrderShipped @event);
    public Task On(OrderClosed @event);

}