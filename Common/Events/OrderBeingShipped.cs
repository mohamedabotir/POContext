using Common.Constants;
using Common.ValueObject;
using System.Text.Json.Serialization;

namespace Common.Events;

public class OrderBeingShipped: DomainEventBase
{
    [ JsonConstructor]
    public OrderBeingShipped(Guid purchaseOrderGuid,string poNumber,decimal totalAmount, ActivationStatus activationStatus,string purchaseOrderNumber,
        PurchaseOrderStage orderStage ,Guid shippingOrderGuid,long shippingOrderId,string customerName,Address customerAddress,string customerPhoneNumber) :base(nameof(OrderBeingShipped))
    {
        PoNumber = poNumber;
        PurchaseOrderGuid = purchaseOrderGuid;
        TotalAmount = totalAmount;
        ActivationStatus = activationStatus;
        PurchaseOrderNumber = purchaseOrderNumber;
        OrderStage = orderStage;
        ShippingOrderGuid = shippingOrderGuid;
        ShippingOrderId = shippingOrderId;
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerPhoneNumber = customerPhoneNumber;
    }

    public string PoNumber { get; set; }
    public Guid PurchaseOrderGuid { get; set; }
    public decimal TotalAmount { get; private set; }
    public ActivationStatus ActivationStatus { get; private set; }
    public string PurchaseOrderNumber { get; protected set; }
    public PurchaseOrderStage OrderStage { get; private set; }
    public Guid ShippingOrderGuid { get; private set; }
    public long ShippingOrderId { get; private set; }
    public string CustomerName { get; private set; }
    public Address CustomerAddress { get; private set; }
    public string CustomerPhoneNumber { get; private set; }
}