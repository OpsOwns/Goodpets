using Goodpets.Domain.Abstractions;
using Goodpets.Infrastructure.Security.Abstractions;

namespace Goodpets.Application.User.Commands;

public record RegisterUser(string Email, string Password, string Username) : ICommand;

public class RegisterUserHandler : ICommandHandler<RegisterUser>
{
    private readonly IUserAccountRepository _repository;
    private readonly IUserService _registerUserService;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public RegisterUserHandler(IUserAccountRepository repository, IUserService registerUserService,
        IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _registerUserService = registerUserService;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task HandleAsync(RegisterUser command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var user = await _registerUserService.RegisterUser(command.Username, command.Password, command.Email,
            cancellationToken);

        await _repository.Add(user, cancellationToken);
    }
}