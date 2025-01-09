namespace Application.EventHandlers;

public interface IDomainEventHandler<T>
{
    Task HandleAsync (T @event, CancellationToken cancellationToken = default);
}