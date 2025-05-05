using System.Text.Json.Serialization;
using Common.Entity;
using Common.Events;
using Common.Utils;
using Common.ValueObject;
using Domain.Entity;

namespace Domain.DomainEvents;

[method: JsonConstructor]
public class PoCreatedEventBase(
    long internalPoId,
    Guid poGuid,
    IReadOnlyList<LineItem> lineItems,
    Money totalAmount,
    User customer,
    User supplier,
    PoNumber poNumber)
    : DomainEventBase(nameof(PoCreatedEventBase))
{
    public User Supplier { init; get; } = supplier;


    public User Customer { init; get; } = customer;

    public Money TotalAmount { init; get; } = totalAmount;

    public IReadOnlyList<LineItem> LineItems { init; get; } = lineItems;

    public Guid PoGuid { init; get; } = poGuid;

    public long InternalPoId{init; get; } = internalPoId;
    public string PoNumber { get; set; } = poNumber.PoNumberValue;
}
