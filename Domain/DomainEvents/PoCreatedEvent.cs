using Domain.Entity;
using Domain.ValueObject;

namespace Domain.DomainEvents;

public record PoCreatedEvent(long InternalPoId,Guid PoGuid,IReadOnlyList<LineItem>LineItems,decimal TotalAmount, User Customer, User Supplier) : IDomainEvent;
