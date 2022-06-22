namespace Goodpets.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }

    private Address(string city, string street, string zipCode)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }


    public static Result<Address> Create(string city, string street, string zipCode)
    {
        if (string.IsNullOrEmpty(city))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(city)));
        if (string.IsNullOrEmpty(street))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(street)));
        if (string.IsNullOrEmpty(zipCode))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(zipCode)));

        return Result.Ok(new Address(city, street, zipCode));
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }
}