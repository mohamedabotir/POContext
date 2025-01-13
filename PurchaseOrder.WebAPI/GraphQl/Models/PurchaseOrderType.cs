using Domain.Entity;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public class PurchaseOrderType : ObjectGraphType<PoEntity>
{
    public PurchaseOrderType()
    {
        Field(x => x.Id).Description("The ID of the purchase order.");
        Field(x => x.PoNumber.PoNumberValue).Description("The purchase order number.");
        Field(x => x.TotalAmount.MoneyValue).Description("The total amount of the purchase order.");
        Field<ListGraphType<LineItemsType>>("lineItems", "The list of line items.");
    }
}