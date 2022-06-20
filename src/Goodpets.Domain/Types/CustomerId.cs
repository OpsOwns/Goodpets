namespace Goodpets.Domain.Types;

public record CustomerId : EntityId
{
    public CustomerId(Guid value) : base(value)
    {
    }

    public CustomerId()
    {
    }
}