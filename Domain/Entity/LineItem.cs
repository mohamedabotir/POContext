using Domain.ValueObject;

namespace Domain.Entity;

public class LineItem :Common.Entity.Entity
{
    public LineItem(Quantity quantity,Item item,Guid guid,long id)
    {
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Guid = guid;  
        Id = id;
    }

    public LineItem()
    {
        
    }
    public virtual Guid Guid { get; protected set; }
    public virtual Quantity Quantity { get; protected set; }
    public virtual int PurchaseOrderId { get; protected set; }
    public Item Item { get; private set; }
}