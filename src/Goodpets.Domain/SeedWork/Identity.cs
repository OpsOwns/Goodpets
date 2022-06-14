namespace Goodpets.Domain.SeedWork;

public record Identity
{
    public Guid Value { get; }
    public Identity() => Value = Guid.NewGuid();
    protected Identity(Guid value) => Value = value;
    public override string ToString() => Value.ToString();
    public static implicit operator Identity(Guid identity) => new(identity);
    public static implicit operator Guid(Identity identity) => identity.Value;
}