namespace Goodpets.Domain.Entities;

public sealed class Owner : Entity, IAggregateRoot
{
    public CustomerId CustomerId { get; private set; }
    public FullName FullName { get; private set; }
    public UserId UserId { get; private set; }
    public Email ContactEmail { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private readonly List<Pet> _pets = new();
    public IReadOnlyList<Pet> Pets => _pets.ToList();

    private Owner()
    {
        CustomerId = null!;
        UserId = null!;
        FullName = null!;
        ContactEmail = null!;
        Address = null!;
        PhoneNumber = null!;
    }


    public Result AssignPet(Pet pet)
    {
        if (_pets.Any(x => x.PetId == pet.PetId))
        {
            return Result.Fail(new Error("This pet is already assigment to owner").WithErrorCode(nameof(Pet)));
        }

        _pets.Add(pet);

        return Result.Ok();
    }

    public static Result<Owner> Register(UserId userId, Email email, Address address, FullName fullName,
        PhoneNumber phoneNumber)
    {
        var customer = new Owner();

        var resultUserId = customer.ChangeUserId(userId);
        var resultPhoneNumber = customer.ChangePhoneNumber(phoneNumber);
        var resultAddress = customer.ChangeAddress(address);
        var resultEmail = customer.ChangeContactEmail(email);
        var resultFullName = customer.ChangeFullName(fullName);

        var mergedResult = Result.Merge(resultUserId, resultPhoneNumber, resultAddress, resultEmail, resultFullName);

        return mergedResult.IsFailed ? mergedResult : Result.Ok(customer);
    }

    public Result ChangeFullName(FullName? fullName)
    {
        if (fullName is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(FullName)));

        FullName = fullName;

        return Result.Ok();
    }

    public Result ChangeContactEmail(Email? contactEmail)
    {
        if (contactEmail is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(contactEmail)));

        ContactEmail = contactEmail;

        return Result.Ok();
    }

    public Result ChangeAddress(Address? address)
    {
        if (address is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(address)));

        Address = address;

        return Result.Ok();
    }

    public Result ChangePhoneNumber(PhoneNumber? phoneNumber)
    {
        if (phoneNumber is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(phoneNumber)));

        PhoneNumber = phoneNumber;

        return Result.Ok();
    }

    public Result ChangeUserId(UserId? userId)
    {
        if (userId is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(userId)));

        UserId = userId;

        return Result.Ok();
    }
}