namespace Goodpets.Domain.Base.Types;

public record EntityId
{
    public EntityId()
    {
        Value = Guid.NewGuid();
    }

    protected EntityId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator EntityId(Guid identity)
    {
        return new(identity);
    }

    public static implicit operator Guid(EntityId identity)
    {
        return identity.Value;
    }
}