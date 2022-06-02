namespace Goodpets.Shared.Domain;

public abstract record Identity(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}