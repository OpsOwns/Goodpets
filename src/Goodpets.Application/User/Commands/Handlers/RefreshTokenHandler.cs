namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class RefreshTokenHandler : ICommandHandler<RefreshToken>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;
    private readonly IIdentity _identity;

    public RefreshTokenHandler(ITokenProvider tokenProvider, IUserRepository userRepository,
        IIdentity identity)
    {
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
        _identity = identity;
    }

    public async Task<Result> HandleAsync(RefreshToken command, CancellationToken cancellationToken = default)
    {
        _tokenProvider.ValidatePrincipalFromExpiredToken(command.AccessToken);

        var userId = _tokenProvider.GetUserIdFromJwtToken();

        var user = await _userRepository.GetUser(userId, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("This user not exists").WithErrorCode("user"));

        var storedRefreshToken = await _userRepository.GetRefreshToken(command.Token, cancellationToken);

        if (storedRefreshToken is null)
            return Result.Fail(new Error("This refresh token not exists").WithErrorCode("refreshToken"));

        if (SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime > storedRefreshToken.ExpireDate)
            return Result.Fail(new Error("This refresh token has expired").WithErrorCode("refreshToken"));

        if (storedRefreshToken.Used)
            return Result.Fail(new Error("This refresh token has been used").WithErrorCode("refreshToken"));


        if (!_tokenProvider.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
            return Result.Fail(new Error("This refresh token does not match this JWT").WithErrorCode("refreshToken"));

        storedRefreshToken = Token.Create(storedRefreshToken.RefreshToken, storedRefreshToken.ExpireDate, true,
            storedRefreshToken.JwtId).Value;

        user.ChangeToken(storedRefreshToken);

        await _userRepository.UpdateUser(user);

        var jwtToken = _tokenProvider.GenerateJwtToken(user);
        var refreshTokenNew = _tokenProvider.GenerateRefreshToken();

        _identity.Set(new JsonWebToken(jwtToken.Value, refreshTokenNew.Value));

        return Result.Ok();
    }
}