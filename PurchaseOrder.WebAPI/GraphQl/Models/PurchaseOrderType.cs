using Common.Entity;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public sealed class PurchaseOrderType : ObjectGraphType<PoEntity>
{
    public PurchaseOrderType()
    {
        Field(x => x.Id).Description("The ID of the purchase order.");
        Field(x => x.PoNumber.PoNumberValue).Description("The purchase order number.");
        Field(x => x.TotalAmount.MoneyValue).Description("The total amount of the purchase order.");
        Field<IntGraphType>("activationStatus")
            .Resolve(context => (int)context.Source.ActivationStatus)
            .Description("The activation status of the purchase order as an integer.");
        Field<ListGraphType<LineItemsType>>("lineItems").Description("The list of line items.");
    }
}