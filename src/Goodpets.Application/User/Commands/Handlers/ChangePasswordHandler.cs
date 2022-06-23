namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class ChangePasswordHandler : ICommandHandler<ChangePassword>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentity _identity;
    private readonly IPasswordManager _passwordManager;

    public ChangePasswordHandler(IUserRepository userRepository, IIdentity identity, IPasswordManager passwordManager)
    {
        _userRepository = userRepository;
        _identity = identity;
        _passwordManager = passwordManager;
    }

    public async Task<Result> HandleAsync(ChangePassword command, CancellationToken cancellationToken = default)
    {
        var oldPassword = Password.Create(command.OldPassword);
        var newPassword = Password.Create(command.NewPassword);

        var result = Result.Merge(oldPassword, newPassword);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var user = await _userRepository.GetUser(_identity.UserId, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not found in the system").WithErrorCode("User"));


        if (!_passwordManager.Validate(oldPassword.Value, user.Password.Value))
        {
            return Result.Fail(new Error("Old password is invalid").WithErrorCode("oldPassword"));
        }

        var encryptPassword = _passwordManager.Encrypt(newPassword.Value);

        user.ChangePassword(Password.Create(encryptPassword).Value);

        await _userRepository.UpdateUser(user);

        return Result.Ok();
    }
}