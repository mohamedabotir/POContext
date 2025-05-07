using Common.Repository;
using GraphQL;
using GraphQL.Types;
using Infrastructure.Utils;

namespace PurchaseOrder.WebAPI.GraphQl;

public sealed class PurchaseOrderQuery: ObjectGraphType
{
    public PurchaseOrderQuery(IPurchaseOrderRepository repository, TopPoCacheService topPoCacheService)
    {
        Field<ListGraphType<PurchaseOrderType>>("purchaseOrders")
            .ResolveAsync(async context => await repository.GetAllAsync());
        Field<ListGraphType<PurchaseOrderType>>("top7PurchaseOrder")
            .ResolveAsync(async context => await topPoCacheService.GetTop7PosAsync());
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