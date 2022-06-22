﻿using Result = FluentResults.Result;

namespace Goodpets.Application.Commands.Handlers;

public class RegisterCustomerHandler : ICommandHandler<RegisterCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IIdentity _identity;

    public RegisterCustomerHandler(IIdentity identity, ICustomerRepository customerRepository)
    {
        _identity = identity;
        _customerRepository = customerRepository;
    }

    public async Task<Result> HandleAsync(RegisterCustomer command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.ContactEmail);
        var address = Address.Create(command.City, command.Street, command.ZipCode);
        var fullName = FullName.Create(command.Name, command.SureName);
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber);

        var resultOfCreateValueObjects = Result.Merge(email, address, fullName, phoneNumber);

        if (resultOfCreateValueObjects.IsFailed)
            return resultOfCreateValueObjects;

        var owner = Owner.Register(_identity.UserId, email.Value, address.Value, fullName.Value,
            phoneNumber.Value);

        if (owner.IsFailed)
            return owner.ToResult();

        await _customerRepository.Register(owner.Value, cancellationToken);

        return Result.Ok();
    }
}