namespace Goodpets.Application.Commands.Handlers;

public class RegisterClientHandler : ICommandHandler<RegisterClient>
{
    private readonly IClientRepository _clientRepository;
    private readonly IIdentity _identity;

    public RegisterClientHandler(IClientRepository clientRepository, IIdentity identity)
    {
        _clientRepository = clientRepository;
        _identity = identity;
    }

    public async Task<Result> HandleAsync(RegisterClient command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.ContactEmail);

        if (email.IsFailed)
            return email.ToResult();

        var address = Address.Create(command.City, command.Street, command.ZipCode);

        if (address.IsFailed)
            return address.ToResult();

        var client = new Client();

        client.Register(_identity.UserAccountId, email.Value, address.Value, command.PhoneNumber);


        await _clientRepository.RegisterClient(client, cancellationToken);

        return Result.Ok();
    }
}