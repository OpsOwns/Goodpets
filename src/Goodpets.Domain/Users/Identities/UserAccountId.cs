namespace Goodpets.Domain.Users.Identities;

public record UserAccountId : Identity
{
    public UserAccountId(Guid value) : base(value)
    {
    }

    public UserAccountId()
    {
    }
}