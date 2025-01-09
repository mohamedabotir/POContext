using Domain.Entity;
using Domain.ValueObject;

namespace Domain.DomainEvents;

public  class  PoCreatedEventBase : DomainEventBase
{
    public PoCreatedEventBase(long internalPoId,Guid poGuid,IReadOnlyList<LineItem> lineItems, decimal totalAmount, User customer,User supplier)
    {
        Supplier = supplier;
        Customer = customer;
        TotalAmount = totalAmount;
        LineItems = lineItems;
        PoGuid = poGuid;
        InternalPoId = internalPoId;
    }


    public User Supplier { set; get; }


    public User Customer { set; get; }

    public decimal TotalAmount { set; get; }

    public IReadOnlyList<LineItem> LineItems { set; get; }

    public Guid PoGuid { set; get; }

    public long InternalPoId{set; get; }

   
}
