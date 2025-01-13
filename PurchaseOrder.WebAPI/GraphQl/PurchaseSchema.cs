using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public class PurchaseSchema: Schema
{
    public PurchaseSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<PurchaseOrderQuery>();
    }
}