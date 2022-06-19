namespace Goodpets.Domain.Types;

public record ClientId : EntityId
{
    public ClientId(Guid value) : base(value)
    {
    }

    public ClientId()
    {
    }
}