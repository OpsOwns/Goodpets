namespace Goodpets.Application.User.Commands;

public record RegisterUser(string Email, string Password, string Username) : ICommand<Result>;

public class RegisterUserHandler : ICommandHandler<RegisterUser, Result>
{
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IUserService _registerUserService;
    private readonly IUserAccountRepository _repository;

    public RegisterUserHandler(IUserAccountRepository repository, IUserService registerUserService,
        IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _registerUserService = registerUserService;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task<Result> HandleAsync(RegisterUser command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var user = await _registerUserService.RegisterUser(command.Username, command.Password, command.Email,
            cancellationToken);

        if (user.IsFailed) return user.ToResult();

        await _repository.Add(user.Value, cancellationToken);

        return Result.Ok();
    }
}