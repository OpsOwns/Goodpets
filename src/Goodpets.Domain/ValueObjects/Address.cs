namespace Goodpets.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }

    private Address(string city, string street, string zipCode)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }


    public static Result<Address> Create(string city, string street, string zipCode)
    {
        if (string.IsNullOrEmpty(city))
            return Result.Fail(new Error("city can't be null or empty").WithMetadata("ErrorParameter", nameof(city)));
        if (string.IsNullOrEmpty(street))
            return Result.Fail(
                new Error("street can't be null or empty").WithMetadata("ErrorParameter", nameof(street)));
        if (string.IsNullOrEmpty(zipCode))
            return Result.Fail(
                new Error("zipCode can't be null or empty").WithMetadata("ErrorParameter", nameof(zipCode)));

        return Result.Ok(new Address(city, street, zipCode));
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }
}