using Common.ValueObject;

namespace Domain.Entity;

public sealed class LineItem :Common.Entity.Entity
{
    public LineItem(Quantity quantity,Item item,Guid guid,long id, int purchaseOrderId)
    {
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Guid = guid;
        PurchaseOrderId = purchaseOrderId;
        Id = id;
    }

    public Guid Guid { get; internal set; }
    public Quantity Quantity { get; internal set; }
    public int PurchaseOrderId { get;internal  set; }
    public Item Item { get; internal set; }
}