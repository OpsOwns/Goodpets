namespace Goodpets.Domain.Types;

public record UserId : EntityId
{
    public UserId(Guid value) : base(value)
    {
    }

    public UserId()
    {
    }

    public static implicit operator UserId(string userAccountId)
    {
        if (string.IsNullOrEmpty(userAccountId))
            throw new ArgumentNullException(nameof(userAccountId));

        if (!Guid.TryParse(userAccountId, out var userId))
        {
            throw new InvalidCastException(nameof(userAccountId));
        }

        return new UserId(userId);
    }
}