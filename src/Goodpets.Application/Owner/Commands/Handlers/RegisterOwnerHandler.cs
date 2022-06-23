namespace Goodpets.Application.Owner.Commands.Handlers;

internal sealed class RegisterOwnerHandler : ICommandHandler<RegisterOwner>
{
    private readonly IOwnerRepository _customerRepository;
    private readonly IIdentity _identity;

    public RegisterOwnerHandler(IIdentity identity, IOwnerRepository customerRepository)
    {
        _identity = identity;
        _customerRepository = customerRepository;
    }

    public async Task<Result> HandleAsync(RegisterOwner command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.ContactEmail);
        var address = Address.Create(command.City, command.Street, command.ZipCode);
        var fullName = FullName.Create(command.Name, command.SureName);
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber);

        var resultOfCreateValueObjects = Result.Merge(email, address, fullName, phoneNumber);

        if (resultOfCreateValueObjects.IsFailed)
            return resultOfCreateValueObjects;

        var owner = Domain.Entities.Owner.Register(_identity.UserId, email.Value, address.Value, fullName.Value,
            phoneNumber.Value);

        if (owner.IsFailed)
            return owner.ToResult();

        await _customerRepository.Add(owner.Value, cancellationToken);

        return Result.Ok();
    }
}