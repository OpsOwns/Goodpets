namespace Goodpets.Domain.Users.ValueObjects;

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
            throw new ArgumentNullException(nameof(value));

        value = value.Trim();

        if (value.Length > 200)
            return Result.Fail("Email is too long");

        if (!Regex.IsMatch(value, @"^(.+)@(.+)$"))
            return Result.Fail("Email is invalid");

        return Result.Ok(new Email(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}