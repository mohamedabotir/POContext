namespace Domain.DomainEvents;

public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, DomainEventBase eventBase);
    Task<List<DomainEventBase>> GetEventsAsync(Guid aggregateId,string collectionName="");
}