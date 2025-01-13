using Common.Repository;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public class PurchaseOrderQuery: ObjectGraphType
{
    public PurchaseOrderQuery(IPurchaseOrderRepository repository)
    {
        FieldAsync<ListGraphType<PurchaseOrderType>>(
            "purchaseOrders",
            resolve: async context => await repository.GetAllAsync()
        );
    }
}