using Common.Constants;
using Common.ValueObject;

namespace Common.Events;

public class PurchaseOrderApproved(
    Guid purchaseOrderId,
    string purchaseOrderNumber,
    ActivationStatus activationStatus,
    Money totalAmount,
    string customerName,
    string customerAddress,
    string customerPhoneNumber)
    : DomainEventBase(nameof(PurchaseOrderApproved))
{
    public Guid PurchaseOrderId { get; } = purchaseOrderId;
    public string PurchaseOrderNumber { get;  } = purchaseOrderNumber;
    public ActivationStatus ActivationStatus { get; } = activationStatus;
    public Money TotalAmount { get;} = totalAmount;
    public string CustomerName { get;} = customerName;
    public string CustomerAddress { get;} = customerAddress;
    public string CustomerPhoneNumber { get;} = customerPhoneNumber;
}