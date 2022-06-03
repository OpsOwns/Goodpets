namespace Goodpets.Domain.Users;

public record UserAccountId : Identity
{
    public UserAccountId(Guid value) : base(value)
    {
    }

    public UserAccountId() { }
}