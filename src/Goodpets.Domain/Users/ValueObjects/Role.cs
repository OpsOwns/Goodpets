namespace Goodpets.Domain.Users.ValueObjects;

public class Role : ValueObject
{
    private Role(string value)
    {
        Value = value;
    }

    public static IEnumerable<string> AvailableRoles { get; } = new[] { "vet", "admin", "user" };
    public string Value { get; }

    public static Role Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        value = value.Trim().ToLower();

        if (!AvailableRoles.Contains(value)) throw new BusinessException($"selected role {value} dose not exists");

        return new Role(value);
    }

    public static Role Admin()
    {
        return new("admin");
    }

    public static Role User()
    {
        return new("user");
    }

    public static Role Vet()
    {
        return new("vet");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}