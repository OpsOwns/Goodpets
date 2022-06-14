using Goodpets.Domain.Base.Types;

namespace Goodpets.Domain.Users.Identities;

public record UserAccountId : EntityId
{
    public UserAccountId(Guid value) : base(value)
    {
    }

    public UserAccountId()
    {
    }

    public static implicit operator UserAccountId(string jwtId)
    {
        if (string.IsNullOrEmpty(jwtId))
            throw new ArgumentNullException(nameof(jwtId));

        return new UserAccountId(Guid.Parse(jwtId));
    }
}