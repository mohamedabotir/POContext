using Domain.Result;

namespace Domain.ValueObject;

public sealed class User: ValueObject<User>
{
    private User(Email email, string name,string phoneNumber)
    {
       
        Email = email;
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public static Result<User> CreateInstance(Email email, Maybe<string> nameOrNothing, string phoneNumber)
    {
    return    nameOrNothing.ToResult("Name cannot be empty")
            .Ensure(name => name.Length < 256, "Name is too long")
            .Ensure(name => name.Length >= 7, "Name is too short")
            .Map(name => new User(email, name, phoneNumber));
    }
    public Email Email { get; private set; }
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    
    protected override bool EqualsCore(User other) => Email == other.Email && Name == other.Name && PhoneNumber == other.PhoneNumber;

    protected override int GetHashCodeCore()=> HashCode.Combine(Email, Name, PhoneNumber);
}