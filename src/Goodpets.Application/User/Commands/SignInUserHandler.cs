namespace Goodpets.Application.User.Commands;

public record SignIn(string Login, string Password) : ICommand<Result<AccessTokenDto>>;

public class SignInrHandler : ICommandHandler<SignIn, Result<AccessTokenDto>>
{
    private readonly IClock _clock;
    private readonly ITokenRepository _tokenRepository;
    private readonly ITokenProvider _tokenService;
    private readonly IUserService _userService;

    public SignInrHandler(ITokenProvider tokenService, IUserService userService,
        ITokenRepository tokenRepository, IClock clock)
    {
        _tokenService = tokenService;
        _userService = userService;
        _tokenRepository = tokenRepository;
        _clock = clock;
    }


    public async Task<Result<AccessTokenDto>> HandleAsync(SignIn query,
        CancellationToken cancellationToken = default)
    {
        var userAccountResult = await _userService.Login(query.Login, query.Password, cancellationToken);

        if (userAccountResult.IsFailed)
            return userAccountResult.ToResult();

        var userAccount = userAccountResult.Value;

        var accessToken = _tokenService.GenerateJwtToken(userAccount);

        var refreshToken = _tokenService.GenerateRefreshToken();

        var existedUserRefreshToken = await _tokenRepository.GetToken(userAccount.Id, cancellationToken);

        if (existedUserRefreshToken.IsSuccess)
            await _tokenRepository.RemoveToken(existedUserRefreshToken.Value, cancellationToken);

        var userRefreshToken = Token.Create(refreshToken.Value,
            refreshToken.ExpireTime, _clock.Current(), userAccount.Id, accessToken.JwtId, false);

        await _tokenRepository.Create(userRefreshToken, cancellationToken);


        return Result.Ok(new AccessTokenDto(accessToken.Value, refreshToken.Value));
    }
}