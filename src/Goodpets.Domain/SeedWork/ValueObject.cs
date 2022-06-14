namespace Goodpets.Domain.SeedWork;

[Serializable]
public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    public virtual int CompareTo(object? obj)
    {
        var thisType = GetType();
        var otherType = obj?.GetType();

        if (thisType != otherType)
            return string.Compare(thisType.ToString(), otherType?.ToString(), StringComparison.Ordinal);

        var other = (ValueObject)obj!;

        object?[] components = GetEqualityComponents().ToArray();
        object?[] otherComponents = other.GetEqualityComponents().ToArray();

        for (var i = 0; i < components.Length; i++)
        {
            var comparison = CompareComponents(components[i], otherComponents[i]);
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }

    public virtual int CompareTo(ValueObject? other)
    {
        return CompareTo(other as object);
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    protected bool Equals(ValueObject? obj)
    {
        if (obj is null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        var value = obj;
        return GetEqualityComponents().SequenceEqual(value.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ValueObject)obj);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(1, (current, obj) =>
        {
            unchecked
            {
                return current * 23 + obj.GetHashCode();
            }
        });
    }

    private int CompareComponents(object? object1, object? object2)
    {
        if (object1 is null && object2 is null)
            return 0;

        if (object1 is null)
            return -1;

        if (object2 is null)
            return 1;

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            return comparable1.CompareTo(comparable2);

        return object1.Equals(object2) ? 0 : -1;
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }
}