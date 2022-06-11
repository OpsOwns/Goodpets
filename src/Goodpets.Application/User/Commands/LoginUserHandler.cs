using Goodpets.Domain.Users.ValueObjects;
using Goodpets.Infrastructure.Security;

namespace Goodpets.Application.User.Commands;

public record LoginUser(string Login, string Password) : ICommand<Result<AccessTokenDto>>;

public class LoginUserHandler : ICommandHandler<LoginUser, Result<AccessTokenDto>>
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IUserAccountRepository _accountRepository;

    public LoginUserHandler(ITokenService tokenService, IUserService userService,
        IUserAccountRepository accountRepository)
    {
        _tokenService = tokenService;
        _userService = userService;
        _accountRepository = accountRepository;
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


        var refreshTokenResult = userAccount.RefreshToken is null
            ? Token.Create(refreshToken.Value, refreshToken.ExpireTime, false, false)
            : Token.Create(refreshToken.Value, refreshToken.ExpireTime, userAccount.RefreshToken.Invalidated,
                userAccount.RefreshToken.Used);
        
        if (refreshTokenResult.IsFailed)
            return refreshTokenResult.ToResult();

        userAccount.CreateRefreshToken(refreshTokenResult.Value);

        await _accountRepository.UpdateUser(userAccount, cancellationToken);

        return Result.Ok(new AccessTokenDto(accessToken, refreshToken.Value));
    }
}