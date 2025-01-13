using Application.Context.Pocos;
using Common.Entity;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public class LineItemsType : ObjectGraphType<LineItem>
{
    public LineItemsType()
    {
        Field(x => x.Id).Description("The ID of the line item.");
        Field(x => x.Quantity.QuantityType).Description("The name of the line item.");
        Field(x => x.Quantity.QuantityValue).Description("The quantity of the line item.");
        Field(x => x.Item.Name).Description("The quantity of the line item.");
        Field(x => x.Item.Price).Description("The quantity of the line item.");
        Field(x => x.Item.SKU).Description("The quantity of the line item.");

    }
}