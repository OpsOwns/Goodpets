namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;

    public SignUpHandler(IUserRepository userRepository, IPasswordManager passwordManager)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
    }

    public async Task<Result> HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.Email);
        var password = Password.Create(command.Password);
        var role = Role.Create(command.Role);
        var username = Username.Create(command.Username);

        var mergedResult = Result.Merge(email, password, role, username);

        if (mergedResult.IsFailed)
        {
            return mergedResult;
        }

        if (await _userRepository.DoesUserEmailExists(email.Value, cancellationToken))
            return Result.Fail(
                new Error($"User with email {email.Value.Value} already exists in system").WithErrorCode("email"));

        var encryptedPassword = Password.Create(_passwordManager.Encrypt(password.Value)).Value;

        var user = Domain.Entities.User.Create(role.Value, username.Value, encryptedPassword, email.Value);

        if (user.IsFailed)
            return user.ToResult();

        await _userRepository.CreateUser(user.Value, cancellationToken);

        return Result.Ok();
    }
}