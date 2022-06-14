using Goodpets.Domain.Base.Types;

namespace Goodpets.Domain.Users.Identities;

public record TokenId : EntityId
{
    public TokenId(Guid value) : base(value)
    {
    }

    public TokenId()
    {
    }
}