using Domain.DomainEvents;

namespace Infrastructure.Consumer;

public interface IEventHandler
{
    public  Task On(PoCreatedEventBase @event);
}