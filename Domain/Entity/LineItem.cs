using Domain.ValueObject;

namespace Domain.Entity;

public class LineItem :Entity
{
    public LineItem(Quantity quantity,Item item)
    {
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        Item = item ?? throw new ArgumentNullException(nameof(item));
    }

    public Quantity Quantity { get; private set; }
    public int PurchaseOrderId { get; private set; }
    public Item Item { get; private set; }
}