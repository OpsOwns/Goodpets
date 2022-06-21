namespace Goodpets.Domain.ValueObjects;

public class Username : ValueObject
{
    public string Value { get; }

    public Username(string value)
    {
        Value = value;
    }

    public static Result<Username> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 30 or < 3)
        {
            return Result.Fail(
                new Error("username can't be null or empty and can't have greater than 30 letter and less than 3")
                    .WithMetadata("ErrorParameter", nameof(Username)));
        }

        return Result.Ok(new Username(value));
    }


    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}