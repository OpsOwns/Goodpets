namespace Goodpets.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }
    
    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(PhoneNumber)));
        }

        if (!Regex.IsMatch(value, "^([0-9]{9})$"))
        {
            return Result.Fail(new Error("Invalid phone number format").WithMetadata("ErrorCode", nameof(PhoneNumber)));
        }

        return Result.Ok(new PhoneNumber(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}