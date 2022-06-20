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
            return Result.Fail("Phone number can't be null or empty");
        }

        if (!Regex.IsMatch(value, "^([0-9]{9})$"))
        {
            return Result.Fail("Invalid phone number format");
        }

        return Result.Ok(new PhoneNumber(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}