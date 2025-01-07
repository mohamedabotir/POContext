using Domain.DomainEvents;

namespace Domain.Entity;

public class AggregateRoot : Entity
{
    public virtual Guid Guid { get; protected internal set; }
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    protected virtual void AddDomainEvent(IDomainEvent newEvent)
    {
        _domainEvents.Add(newEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }
}