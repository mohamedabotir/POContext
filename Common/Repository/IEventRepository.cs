using Common.Entity;
using Common.Events;

namespace Common.Repository;

public interface IEventRepository
{
    Task SaveEventAsync(string aggregateId, IEnumerable<DomainEventBase> baseEvents, int expectedVersion, bool byName = false, string topicName = "", string collectionName = "");
    Task<List<EventModel>> GetAggregate(string aggregateId);
    Task<List<EventModel>> GetUnprocessedEventsAsync();
    Task MarkAsProcessed(string eventId);
}