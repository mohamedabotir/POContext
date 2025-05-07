using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entity;
using Common.ValueObject;
using Domain.Entity;

namespace Application.Context.Pocos;

[Table("LineItem")]
public class LineItems
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int Id { get;    set; }

    [Required]
    public  Guid Guid { get; set; }

    [Required]
    public int QuantityValue { get; set; }

    [Required]
    public QuantityType QuantityType { get; set; }

    [Required]
    [ForeignKey("PurchaseOrder")]
    public  int PurchaseOrderId { get; set; }

    [Required]
    [MaxLength(50)]
    public  string Name { get; set; }

    [Required]
    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(20)]
    public  string SKU { get; set; }

    [ForeignKey("PurchaseOrderId")]
    public virtual PurchaseOrder PurchaseOrder { get; set; }

    public LineItem MapLineItemPocoToItemLine()
    {
        return new LineItem(Quantity.CreateInstance(QuantityValue, QuantityType).Value,new Item(Name, Price, SKU),Guid,(int)Id,PurchaseOrderId);
    }

    public LineItems MapItemLineToLineItemPoco(LineItem item)
    {
        return new LineItems()
        {
         Id = (int)item.Id,
         Guid = item.Guid,
         PurchaseOrderId = item.PurchaseOrderId,
         QuantityValue = item.Quantity.QuantityValue,
         QuantityType = item.Quantity.QuantityType,
         Name = item.Item.Name,
         Price = item.Item.Price,
         SKU = item.Item.SKU,
        };
    }
}