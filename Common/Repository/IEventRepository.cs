using Common.Domains;

namespace Domain.Repository;

public interface IEventRepository
{
    Task SaveEventAsync(EventModel @event);
    Task<List<EventModel>> GetAggregate(Guid aggregateId);
    Task<List<EventModel>> GetAggregateByUserName(string name);
}