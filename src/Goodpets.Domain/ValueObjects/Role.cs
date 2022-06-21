namespace Goodpets.Domain.ValueObjects;

public class Role : ValueObject
{
    public string Value { get; }
    public static IEnumerable<string> AvailableRoles { get; } = new[] { "user", "admin" };

    private Role(string value)
    {
        Value = value;
    }

    public static Result<Role> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 30)
        {
            return Result.Fail(new Error("role can't be null or empty").WithMetadata("ErrorParameter", nameof(Role)));
        }

        if (!AvailableRoles.Contains(value))
        {
            return Result.Fail(
                new Error($"invalid role only available roles are {string.Join(',', AvailableRoles)}").WithMetadata(
                    "ErrorParameter", nameof(Role)));
        }

        return Result.Ok(new Role(value));
    }

    public static Role Admin() => new("admin");

    public static Role User() => new("user");

    public static implicit operator Role(string value) => new(value);

    public static implicit operator string(Role value) => value.Value;

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}