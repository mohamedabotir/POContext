using Common.Repository;
using GraphQL;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public sealed class PurchaseOrderQuery: ObjectGraphType
{
    public PurchaseOrderQuery(IPurchaseOrderRepository repository)
    {
        Field<ListGraphType<PurchaseOrderType>>("purchaseOrders")
            .ResolveAsync(async context => await repository.GetAllAsync());
        Field<PurchaseOrderType>("purchaseOrderByPurchaseOrderNumber")
            .Arguments(new QueryArguments(new QueryArgument<StringGraphType> { Name = "purchaseOrderNumber" }))
            .ResolveAsync(async context =>
            {
                var purchaseNumber = context.GetArgument<string>("purchaseOrderNumber");
                var result = await repository.GetPoByPurchaseNumberAsync(purchaseNumber);
                return result.Value;
            });
    }
}