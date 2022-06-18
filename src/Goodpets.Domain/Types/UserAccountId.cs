using Goodpets.Domain.Base;

namespace Goodpets.Domain.Types;

public record UserAccountId : EntityId
{
    public UserAccountId(Guid value) : base(value)
    {
    }

    public UserAccountId()
    {
    }

    public static implicit operator UserAccountId(string userAccountId)
    {
        if (string.IsNullOrEmpty(userAccountId))
            throw new ArgumentNullException(nameof(userAccountId));

        if (!Guid.TryParse(userAccountId, out var userId))
        {
            throw new InvalidCastException(nameof(userAccountId));
        }

        return new UserAccountId(userId);
    }
}