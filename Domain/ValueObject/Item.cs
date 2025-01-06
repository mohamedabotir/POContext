namespace Domain.ValueObject;

public class Item: ValueObject<Item>
{
    public Item(string name, decimal price, string sku)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Price = price;
        SKU = sku ?? throw new ArgumentNullException(nameof(sku));
    }

    public string Name { get; }
    public decimal Price { get; }// add money value object 
    public string SKU { get; } // Stock Keeping Unit, a unique identifier

    protected override bool EqualsCore(Item other)
    {
      return other.Name == Name && other.Price == Price && other.SKU == SKU;
    }

    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
}