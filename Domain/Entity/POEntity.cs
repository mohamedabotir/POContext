using Domain.DomainEvents;
using Domain.Handlers;
using Domain.ValueObject;

namespace Domain.Entity;

public class PoEntity : AggregateRoot
{
    public PoEntity(Money totalAmount,Guid rootGuid,User customer,User supplier)
    {
       
        if(Guid.Empty == rootGuid)
            throw new ArgumentException(nameof(rootGuid));
        TotalAmount = totalAmount;
        base.Guid = rootGuid;
        base.CreatedOn = DateTime.UtcNow;
        Customer = customer;
        Supplier = supplier;
    }
 
    public Money TotalAmount { get; protected set; }
    public User Customer { get; protected set; }
    public User Supplier { get; protected set; }
    public virtual ICollection<LineItem> LineItems { get; protected set; } = new List<LineItem>();

    public Result.Result AddLineItems(List<LineItem> lineItem)
    {
        foreach (var line in lineItem)
        {
            if (LineItems.Any(l => l.Item == line.Item))
                return Result.Result.Fail("Item already added");
            LineItems.Add(line);
        }
        AddDomainEvent(new PoCreatedEventBase(Id, Guid,(IReadOnlyList<LineItem>)LineItems, TotalAmount,Customer,Supplier));
       return Result.Result.Ok();
    }
}
