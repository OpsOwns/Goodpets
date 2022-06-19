using Goodpets.Domain.Exceptions;

namespace Goodpets.Domain.Entities;

public class Client : Entity<ClientId>
{
    public UserId UserId { get; private set; }
    public Email ContactEmail { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }

    public Client()
    {
        UserId = null!;
        ContactEmail = null!;
        Address = null!;
        PhoneNumber = null!;
    }

    public void Register(UserId userId, Email email, Address address, string phoneNumber)
    {
        if (userId is null || userId.Value == Guid.Empty)
        {
            throw new BusinessException($"{nameof(userId)} can't be null or empty");
        }

        if (string.IsNullOrEmpty(phoneNumber))
            throw new BusinessException($"{nameof(phoneNumber)} can't be null or empty");

        ContactEmail = email ?? throw new BusinessException($"{nameof(email)} can't be null or empty");
        UserId = userId;
        Address = address ?? throw new BusinessException($"{nameof(address)} can't be null or empty");
        PhoneNumber = phoneNumber;
        Id = new ClientId();
    }
}