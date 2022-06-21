namespace Goodpets.Domain.ValueObjects;

public class Password : ValueObject
{
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 200 or < 6)
        {
            return Result.Fail(
                new Error("password can't be null or empty and can't have greater than 200 letter and less than 6")
                    .WithMetadata("ErrorParameter", nameof(Password)));
        }

        return Result.Ok(new Password(value));
    }

    public static implicit operator string(Password value) => value.Value;

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}