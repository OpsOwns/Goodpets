namespace Goodpets.Domain.Users.ValueObjects;

public class Role : ValueObject
{
    public static IEnumerable<string> AvailableRoles { get; } = new[] { "vet", "admin", "user" };
    public string Value { get; }

    private Role(string value)
    {
        Value = value;
    }

    public static Role Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        value = value.Trim().ToLower();

        if (!AvailableRoles.Contains(value))
        {
            throw new BusinessException($"selected role {value} dose not exists");
        }

        return new(value);
    }

    public static Role Admin() => new("admin");

    public static Role User() => new("user");

    public static Role Vet() => new("vet");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}