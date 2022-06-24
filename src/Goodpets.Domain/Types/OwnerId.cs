namespace Goodpets.Domain.Types;

public record OwnerId : EntityId
{
    public OwnerId(Guid value) : base(value)
    {
    }

    public OwnerId()
    {
    }
}