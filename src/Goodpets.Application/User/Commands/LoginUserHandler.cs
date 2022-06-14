namespace Goodpets.Application.User.Commands;

public record LoginUser(string Login, string Password) : ICommand<Result<AccessTokenDto>>;

public class LoginUserHandler : ICommandHandler<LoginUser, Result<AccessTokenDto>>
{
    private readonly ITokenProvider _tokenService;
    private readonly IUserService _userService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IClock _clock;

    public LoginUserHandler(ITokenProvider tokenService, IUserService userService,
        ITokenRepository tokenRepository, IClock clock)
    {
        _tokenService = tokenService;
        _userService = userService;
        _tokenRepository = tokenRepository;
        _clock = clock;
    }


    public async Task<Result<AccessTokenDto>> HandleAsync(LoginUser query,
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
        {
            await _tokenRepository.RemoveToken(existedUserRefreshToken.Value, cancellationToken);
        }

        var userRefreshToken = Token.Create(refreshToken.Value,
            refreshToken.ExpireTime, _clock.Current(), userAccount.Id, accessToken.JwtId, false);

        if (userRefreshToken.IsFailed)
            return userRefreshToken.ToResult();


        await _tokenRepository.Create(userRefreshToken.Value, cancellationToken);


        return Result.Ok(new AccessTokenDto(accessToken.Value, refreshToken.Value));
    }
}