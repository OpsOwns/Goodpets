namespace Goodpets.Domain.ValueObjects;

public class FullName : ValueObject
{
    public string Name { get; }
    public string SureName { get; }

    private FullName(string name, string sureName)
    {
        Name = name;
        SureName = sureName;
    }

    public static Result<FullName> Create(string name, string sureName)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Fail(new Error("name can't be null or empty").WithMetadata("ErrorParameter", nameof(name)));

        if (string.IsNullOrEmpty(sureName))
            return Result.Fail(
                new Error("sureName can't be null or empty").WithMetadata("ErrorParameter", nameof(sureName)));

        return Result.Ok(new FullName(name, sureName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return SureName;
    }
}