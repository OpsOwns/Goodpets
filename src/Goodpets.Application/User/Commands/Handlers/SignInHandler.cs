namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentity _identity;

    public SignInHandler(IUserRepository userRepository, IPasswordManager passwordManager, ITokenProvider tokenProvider,
        IIdentity identity)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _tokenProvider = tokenProvider;
        _identity = identity;
    }

    public async Task<Result> HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        var username = Username.Create(command.UserName);
        var password = Password.Create(command.Password);

        var mergeResult = Result.Merge(username, password);

        if (mergeResult.IsFailed)
        {
            return mergeResult;
        }

        var user = await _userRepository.GetUser(username.Value, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not exists").WithErrorCode("user"));

        var passwordValid =
            _passwordManager.Validate(password.Value, user.Password);

        if (!passwordValid)
            return Result.Fail(new Error("Invalid password").WithErrorCode("password"));

        var accessToken = _tokenProvider.GenerateJwtToken(user);

        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var token = Token.Create(refreshToken.Value, refreshToken.ExpireTime, false,
            accessToken.JwtId);

        if (token.IsFailed)
            return token.ToResult();

        user.ChangeToken(token.Value);

        await _userRepository.UpdateUser(user);

        _identity.Set(new JsonWebToken(accessToken.Value, refreshToken.Value));

        return Result.Ok();
    }
}