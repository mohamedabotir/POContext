using Domain.DomainEvents;
using Domain.Handlers;
using Domain.Result;
using Domain.ValueObject;

namespace Domain.Entity;

public class PoEntity : AggregateRoot
{
    public PoEntity(Money totalAmount,Guid rootGuid,User customer,User supplier,PoNumber poNumber)
    {
       
        if(Guid.Empty == rootGuid)
            throw new ArgumentException(nameof(rootGuid));
        TotalAmount = totalAmount;
        base.Guid = rootGuid;
        base.CreatedOn = DateTime.UtcNow;
        Customer = customer;
        Supplier = supplier;
        PoNumber = poNumber;
    }
 
    public Money TotalAmount { get; protected set; }
    public User Customer { get; protected set; }
    public User Supplier { get; protected set; }
    public ActivationStatus ActivationStatus { get; protected set; } = ActivationStatus.Active;

    public Result.Result DeActivate() => new PoActivationProcessor(new PoDeActivationState(), ActivationStatus).ProcessOrder();
    public Result.Result Activate() => new PoActivationProcessor(new PoActivationState(), ActivationStatus).ProcessOrder();

     
    public PoNumber PoNumber { get; protected set; }
    public virtual ICollection<LineItem> LineItems { get; protected set; } = new List<LineItem>();
    
    public Result.Result AddLineItems(List<LineItem> lineItem)
    {
        foreach (var line in lineItem)
        {
            if (LineItems.Any(l => l.Item == line.Item))
                return Result.Result.Fail("Item already added");
            LineItems.Add(line);
        }
        AddDomainEvent(new PoCreatedEventBase(Id, Guid,(IReadOnlyList<LineItem>)LineItems, TotalAmount,Customer,Supplier));
       return Result.Result.Ok();
    }
}

public enum ActivationStatus
{
    Active,NotActive
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