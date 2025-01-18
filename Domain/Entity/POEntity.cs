using Common.Constants;
using Common.DomainEvents;
using Common.Events;
using Common.Entity;
using Common.Result;
using Common.Utils;
using Common.ValueObject;

namespace Common.Entity;

public class PoEntity : AggregateRoot
{
    public PoEntity(Guid rootGuid,User customer,User supplier,PoNumber poNumber,PurchaseOrderStage stage)
    {
        if(Guid.Empty == rootGuid)
            throw new ArgumentException(nameof(rootGuid));

        base.Guid = rootGuid;
        base.CreatedOn = DateTime.UtcNow;
        Customer = customer;
        Supplier = supplier;
        PoNumber = poNumber;
        PurchaseOrderStage = stage;
    }

    public PoEntity()
    {
        
    }

    public PurchaseOrderStage PurchaseOrderStage { get;private set; }
    public virtual Money TotalAmount { get; protected set; }
    public virtual User Customer { get; protected set; }
    public  User Supplier { get; protected set; }
    public virtual ActivationStatus ActivationStatus { get;  set; } = ActivationStatus.Active;

    public Result.Result DeActivate() => new PoActivationProcessor(new PoDeActivationState(), ActivationStatus).ProcessOrder();
    public Result.Result Activate() => new PoActivationProcessor(new PoActivationState(), ActivationStatus).ProcessOrder();

    public Result.Result ApprovePurchase()
    {
        if(PurchaseOrderStage != PurchaseOrderStage.Created)
            return Result.Result.Fail("PurchaseOrderStage Should be on Created");
        PurchaseOrderStage = PurchaseOrderStage.Approved;
        AddDomainEvent(new PurchaseOrderApproved(Guid,PoNumber.PoNumberValue,ActivationStatus,TotalAmount,Customer.Name,Customer.Address.AddressValue,
            Customer.PhoneNumber,
            PurchaseOrderStage
            ));
        return Result.Result.Ok();
    } 
    public Result.Result ClosePurchaseOrder()
    {
        if(PurchaseOrderStage != PurchaseOrderStage.Shipped)
            return Result.Result.Fail("purchase order  Should be on shipped to close it.");
        PurchaseOrderStage = PurchaseOrderStage.Closed;
        return Result.Result.Ok();
    }
    public virtual PoNumber PoNumber { get; protected set; }
    public  ICollection<LineItem> LineItems { get; protected set; } = new List<LineItem>();
    
    public Result.Result AddLineItems(List<LineItem> lineItem)
    {
        foreach (var line in lineItem)
        {
            if (LineItems.Any(l => l.Item == line.Item))
                return Result.Result.Fail("Item already added");
            LineItems.Add(line);
        }

        var totalAmount = LineItems.Sum(e=>e.Item.Price);
        var moneyAmount = Money.CreateInstance(totalAmount);
        if (moneyAmount.IsFailure)
            return moneyAmount;
        TotalAmount = moneyAmount.Value;
        AddDomainEvent(new PoCreatedEventBase(Id, Guid,(IReadOnlyList<LineItem>)LineItems, TotalAmount,Customer,Supplier));
       return Result.Result.Ok();
    }

     

    public Result.Result SetLineItems(List<LineItem> lineItem)
    { 
            foreach (var line in lineItem)
            {
                LineItems.Add(line);
            }
            var totalAmount = LineItems.Sum(e=>e.Item.Price);
            var moneyAmount = Money.CreateInstance(totalAmount);
            if (moneyAmount.IsFailure)
                return moneyAmount;
            TotalAmount = moneyAmount.Value;
            return Result.Result.Ok();
    }
}

interface IPoActivationState
{
    Result.Result Process(PoActivationProcessor context);
}

 class PoActivationProcessor(IPoActivationState currentState, ActivationStatus activationStatus)
{
    public IPoActivationState CurrentState { get; private set; } = currentState;
    public ActivationStatus ActivationStatus { get; private set;} = activationStatus;

    public void SetState(IPoActivationState state)
    {
        CurrentState = state;
        
    }

    public Result.Result ProcessOrder()
    {
        return CurrentState.Process(this);
    }
}

class PoActivationState: IPoActivationState
{
    public Result.Result Process(PoActivationProcessor context)
    {
        if (context.ActivationStatus != ActivationStatus.NotActive)
            return Result.Result.Fail("Purchase Not On In-Active State");
        return Result.Result.Ok();
    }
}
class PoDeActivationState: IPoActivationState
{
    public Result.Result Process(PoActivationProcessor context)
    {
        if (context.ActivationStatus != ActivationStatus.Active)
            return Result.Result.Fail("Purchase Not on Active State");
        return Result.Result.Ok();
    }
}