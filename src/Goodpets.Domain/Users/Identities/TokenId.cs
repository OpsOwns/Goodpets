namespace Goodpets.Domain.Users.Identities;

public record TokenId : Identity
{
    public TokenId(Guid value) : base(value)
    {
    }

    public TokenId() { }
}