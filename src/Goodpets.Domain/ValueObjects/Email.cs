namespace Goodpets.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }
    
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Fail(new Error("email can't be null or empty").WithMetadata("ErrorParameter", nameof(Email)));

        value = value.Trim();

        if (value.Length > 200)
            return Result.Fail(new Error("Email is too long").WithMetadata("ErrorParameter", nameof(Email)));

        if (!Regex.IsMatch(value, @"^(.+)@(.+)$"))
            return Result.Fail(new Error("Email is invalid").WithMetadata("ErrorParameter", nameof(Email)));

        return Result.Ok(new Email(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}