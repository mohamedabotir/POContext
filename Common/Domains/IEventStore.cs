using Common.Result;

namespace Common.Events;

public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, DomainEventBase eventBase,List<Maybe<string>> anotherTopics = (List<Maybe<string>>)default, int expectedVersion=0);
    
}