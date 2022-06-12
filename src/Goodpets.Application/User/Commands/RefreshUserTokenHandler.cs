using System.Security.Claims;

namespace Goodpets.Application.User.Commands;

public class RefreshTokenHandler
{
    public record RefreshUserToken(string AccessToken, string RefreshToken) : ICommand<Result<AccessTokenDto>>;


    public class RefreshUserTokenHandler : ICommandHandler<RefreshUserToken, Result<AccessTokenDto>>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IClock _clock;

        public RefreshUserTokenHandler(ITokenRepository tokenRepository, ITokenService tokenService,
            IUserAccountRepository accountRepository, IClock clock)
        {
            _tokenRepository = tokenRepository;
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _clock = clock;
        }

        public async Task<Result<AccessTokenDto>> HandleAsync(RefreshUserToken command,
            CancellationToken cancellationToken = default)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(command.AccessToken);

            var expiryDateUnix = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            if (_tokenService.JwtTokenExpired(expiryDateUnix))
                return Result.Fail("This JWT token hasn't expired yet");

            var userId =
                new UserAccountId(Guid.Parse(principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value));

            var userResult = await _accountRepository.GetUserAccount(userId, cancellationToken);

            if (userResult.IsFailed)
            {
                return Result.Fail("This user not exists");
            }

            var storedRefreshTokenResult = await _tokenRepository.GetToken(command.RefreshToken, cancellationToken);

            if (storedRefreshTokenResult.IsFailed)
            {
                return Result.Fail("This refresh token not exists");
            }

            var storedRefreshToken = storedRefreshTokenResult.Value;

            if (_clock.Current() > storedRefreshToken.ExpireDate)
            {
                return Result.Fail("This refresh token has expired");
            }

            if (storedRefreshToken.Invalidated)
                return Result.Fail("This refresh token has been invalidated");

            if (storedRefreshToken.Used)
                return Result.Fail("This refresh token has been used");


            JwtId jwtId = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (jwtId != storedRefreshToken.JwtId)
            {
                return Result.Fail("This refresh token does not match this JWT");
            }

            storedRefreshToken.ChangeStatus(true);

            await _tokenRepository.UpdateToken(storedRefreshToken, cancellationToken);

            var accessToken = _tokenService.GenerateJwtToken(userResult.Value);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Result.Ok(new AccessTokenDto(accessToken.Value, refreshToken.Value));
        }
    }
}