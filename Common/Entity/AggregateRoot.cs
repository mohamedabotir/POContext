using Common.Events;
using System.Reflection;

namespace Common.Entity;

public class AggregateRoot : Entity
{
    private readonly List<DomainEventBase> _uncommittedEvents = new();
    public Guid Guid { get; protected set; }
    public int Version { get;  set; } = -1; // Initial version (empty stream)

    public IReadOnlyList<DomainEventBase> GetUncommittedEvents() => _uncommittedEvents;

    public void MarkChangesAsCommitted()
    {
        _uncommittedEvents.Clear();
    }

    protected void RaiseEvent(DomainEventBase @event)
    {
        ApplyChange(@event, isNew: true);
    }

    public void ReplayEvents(IEnumerable<DomainEventBase> history)
    {
        foreach (var @event in history.OrderBy(e => e.Version)) // optional ordering
        {
            ApplyChange(@event, isNew: false);
            Version++;
        }
    }

    private void ApplyChange(DomainEventBase @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { @event.GetType() }, null);

        if (method == null)
            throw new InvalidOperationException($"Apply method not found for {@event.GetType().Name}");

        method.Invoke(this, new object[] { @event });
        if (isNew)
        {
            _uncommittedEvents.Add(@event);
            Version++;
        }
        else
        {
            Version = @event.Version;
        }
    }
}