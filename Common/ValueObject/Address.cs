using Common.Result;

namespace Common.ValueObject;

public sealed class Address:ValueObject<Address>
{
    public string AddressValue { get; private set; }

    private Address(string address)
    {
        AddressValue = address;
    }
    public static Result<Address> CreateInstance( Maybe<string> addressOrNothing){
       return addressOrNothing.ToResult("Address cannot be empty")
            .Ensure(e=>e.Length>10,"Address must be at least 10 characters!")
            .Map(e=>new Address(e));
    }
    protected override bool EqualsCore(Address other)
    {
      return  other.AddressValue == AddressValue;
    }

    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
}