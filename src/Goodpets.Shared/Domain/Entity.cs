#nullable disable

namespace Goodpets.Shared.Domain;

public abstract class Entity<T> where T : Identity
{
    public bool Exists => Id is not null;
    public T Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(T id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    private bool IsTransient()
    {
        return Equals(default);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<T> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (IsTransient() || other.IsTransient())
            return false;

        return Equals(other);
    }

    public static bool operator ==(Entity<T> a, Entity<T> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<T> a, Entity<T> b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return Id.Value.ToString();
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}