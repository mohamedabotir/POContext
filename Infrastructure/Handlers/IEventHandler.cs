using Common.Events;

namespace Infrastructure.Handlers;

public interface IEventHandler
{
    public Task On(OrderBeingShipped @event);
    public Task On(OrderShipped @event);
    public Task On(OrderClosed @event);

}