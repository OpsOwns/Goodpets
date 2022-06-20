namespace Goodpets.Domain.Entities;

public sealed class Customer : Entity<CustomerId>
{
    public FullName FullName { get; private set; }
    public UserId UserId { get; private set; }
    public Email ContactEmail { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private readonly List<Pet> _pets = new();
    public IReadOnlyList<Pet> Pets => _pets.ToList();

    private Customer()
    {
        UserId = null!;
        FullName = null!;
        ContactEmail = null!;
        Address = null!;
        PhoneNumber = null!;
    }

    public void AssignPet(Pet pet)
    {
        if (_pets.Any(x => x.Id == pet.Id))
        {
            throw new BusinessException("This pet is already assigment to owner");
        }

        _pets.Add(pet);
    }


    public static Customer Register(UserId userId, Email email, Address address, FullName fullName,
        PhoneNumber phoneNumber)
    {
        var customer = new Customer();

        customer.ChangeUserId(userId);
        customer.ChangePhoneNumber(phoneNumber);
        customer.ChangeAddress(address);
        customer.ChangeContactEmail(email);
        customer.ChangeFullName(fullName);

        return customer;
    }

    private void ChangeFullName(FullName fullName)
    {
        FullName = fullName ?? throw new BusinessException($"{nameof(fullName)} can't be null or empty");
    }

    private void ChangeContactEmail(Email email)
    {
        ContactEmail = email ?? throw new BusinessException($"{nameof(email)} can't be null or empty");
    }

    private void ChangeAddress(Address address)
    {
        Address = address ?? throw new BusinessException($"{nameof(address)} can't be null or empty");
    }

    private void ChangePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber ?? throw new BusinessException($"{nameof(phoneNumber)} can't be null or empty");
    }

    private void ChangeUserId(UserId userId)
    {
        if (userId is null || userId.Value == Guid.Empty)
        {
            throw new BusinessException($"{nameof(userId)} can't be null or empty");
        }

        UserId = userId;
    }
}