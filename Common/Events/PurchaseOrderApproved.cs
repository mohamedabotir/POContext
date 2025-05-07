using System.Text.Json.Serialization;
using Common.Constants;
using Common.ValueObject;

namespace Common.Events;


public class PurchaseOrderApproved : DomainEventBase
{
    [ JsonConstructor]
    public PurchaseOrderApproved(Guid purchaseOrderId,
        string purchaseOrderNumber,
        ActivationStatus activationStatus,
        Money totalAmount,
        string customerName,
        string customerAddress,
        string customerPhoneNumber,
        PurchaseOrderStage orderStage ) : base(nameof(PurchaseOrderApproved))
    {
        PurchaseOrderId = purchaseOrderId;
        PurchaseOrderNumber = purchaseOrderNumber;
        ActivationStatus = activationStatus;
        TotalAmount = totalAmount;
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerPhoneNumber = customerPhoneNumber;
        OrderStage = orderStage;
    }

    public Guid PurchaseOrderId { get; private set; }
    public string PurchaseOrderNumber { get; private set; }
    public ActivationStatus ActivationStatus { get; private set; }
    public Money TotalAmount { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerAddress { get; private set; }
    public string CustomerPhoneNumber { get; private set; }
    public PurchaseOrderStage OrderStage { get; private set; }
}