namespace Domain.Entity;

public class PoEntity : AggregateRoot
{
    public PoEntity(decimal totalAmount)
    {
        if (totalAmount < 0)
            throw new ArgumentException(nameof(totalAmount));
        TotalAmount = totalAmount;
    }
 
    public decimal TotalAmount { get; protected set; }
    public DateTime IssuedDate { get; } = DateTime.UtcNow;
    public virtual ICollection<LineItem> LineItems { get; protected set; } = new List<LineItem>();

    public void AddLineItem(List<LineItem> lineItem)
    {
        foreach (var line in lineItem)
        {
            LineItems.Add(line);
        }
    }
}
