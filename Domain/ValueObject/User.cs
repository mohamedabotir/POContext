namespace Domain.ValueObject;

public sealed class User: ValueObject<User>
{
    public User(string email, string name, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw  new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(name))
            throw  new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw  new ArgumentNullException(nameof(phoneNumber));
        Email = email;
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public string Email { get; private set; }
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    
    protected override bool EqualsCore(User other) => Email == other.Email && Name == other.Name && PhoneNumber == other.PhoneNumber;

    protected override int GetHashCodeCore()=> HashCode.Combine(Email, Name, PhoneNumber);
}