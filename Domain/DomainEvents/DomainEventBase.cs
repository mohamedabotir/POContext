namespace Domain.DomainEvents;

public  abstract class DomainEventBase
{
    public DomainEventBase(string type)
    {
        this.Type = type;
    }
    public string Type { get; set;}
};
