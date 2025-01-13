using System.Text.RegularExpressions;
using Domain.Result;

namespace Domain.ValueObject;

public sealed class Email : ValueObject<Email>
{
    public string EmailValue { get; }

    private Email(string email)
    {
        EmailValue = email;
    }
    protected override bool EqualsCore(Email other)
    {
        return other.EmailValue == EmailValue;
    }

    public static Result<Email> CreateInstance(Maybe<string> email)
    {
        return email.ToResult("Email Cannot be empty")
            .OnSuccess(e => e.Trim())
            .Ensure(e => e != string.Empty, $"Email should not be empty :{email}")
            .Ensure(e => e.Length <= 256, $"Email is too long :{email}")
            .Ensure(e => Regex.IsMatch(e, @"^(.+)@(.+)$"), $"Email is invalid :{email}")
            .Map(e=>new Email(e));
    }
    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
}