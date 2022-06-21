namespace Goodpets.Domain.Types;

public record PetId : EntityId
{
    public PetId(Guid value) : base(value)
    {
    }

    public PetId()
    {
    }
}