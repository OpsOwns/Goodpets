using System.Text.RegularExpressions;

namespace Goodpets.Domain.Users.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }


    public static Email Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        value = value.Trim();

        if (value.Length > 200)
            throw new BusinessException("Email is too long");

        if (!Regex.IsMatch(value, @"^(.+)@(.+)$"))
            throw new BusinessException("Email is invalid");

        return new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}