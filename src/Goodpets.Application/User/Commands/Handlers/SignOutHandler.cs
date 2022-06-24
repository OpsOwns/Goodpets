namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class SignOutHandler : ICommandHandler<SignOut>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentity _identity;

    public SignOutHandler(IUserRepository userRepository, IIdentity identity)
    {
        _userRepository = userRepository;
        _identity = identity;
    }

    public async Task<Result> HandleAsync(SignOut command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUser(_identity.UserId, cancellationToken);

        if (user?.Token is null)
            throw new BusinessException("System unable to find user");

        user.RemoveToken();

        await _userRepository.UpdateUser(user);

        return Result.Ok();
    }
}