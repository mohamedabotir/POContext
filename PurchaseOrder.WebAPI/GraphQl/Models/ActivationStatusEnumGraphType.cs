using Common.Constants;
using GraphQL.Types;

namespace PurchaseOrder.WebAPI.GraphQl;

public class ActivationStatusEnumGraphType:EnumerationGraphType<ActivationStatus>
{
    public ActivationStatusEnumGraphType()
    {
        Name = "ActivationStatus";
        Description = "The activation status of a purchase order.";
    }
}