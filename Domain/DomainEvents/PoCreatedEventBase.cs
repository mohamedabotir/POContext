using System.Text.Json.Serialization;
using Common.Domains;
using Common.Entity;
using Common.ValueObject;

namespace Common.DomainEvents;

public  class  PoCreatedEventBase : DomainEventBase
{
    
    [JsonConstructor]
    public PoCreatedEventBase(long internalPoId,Guid poGuid,IReadOnlyList<LineItem> lineItems, Money totalAmount, User customer,User supplier):base(nameof(PoCreatedEventBase))
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

    public Money TotalAmount { set; get; }

    public IReadOnlyList<LineItem> LineItems { set; get; }

    public Guid PoGuid { set; get; }

    public long InternalPoId{set; get; }

   
}
