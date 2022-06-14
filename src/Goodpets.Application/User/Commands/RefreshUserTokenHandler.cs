namespace Goodpets.Application.User.Commands;

public class RefreshTokenHandler
{
    public record RefreshUserToken(string AccessToken, string RefreshToken) : ICommand<Result<AccessTokenDto>>;


    public class RefreshUserTokenHandler : ICommandHandler<RefreshUserToken, Result<AccessTokenDto>>
    {
        private readonly IUserAccountRepository _accountRepository;
        private readonly IClock _clock;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenProvider _tokenService;

        public RefreshUserTokenHandler(ITokenRepository tokenRepository, ITokenProvider tokenService,
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
            _tokenService.ValidatePrincipalFromExpiredToken(command.AccessToken);

            var userId =
                new UserAccountId(_tokenService.GetUserIdFromJwtToken());

            var userResult = await _accountRepository.GetUserAccount(userId, cancellationToken);

            if (userResult.IsFailed) return Result.Fail("This user not exists");

            var storedRefreshTokenResult = await _tokenRepository.GetToken(command.RefreshToken, cancellationToken);

            if (storedRefreshTokenResult.IsFailed) return Result.Fail("This refresh token not exists");

            var storedRefreshToken = storedRefreshTokenResult.Value;

            if (_clock.Current() > storedRefreshToken.ExpireDate) return Result.Fail("This refresh token has expired");

            if (storedRefreshToken.Used)
                return Result.Fail("This refresh token has been used");


            if (!_tokenService.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
                return Result.Fail("This refresh token does not match this JWT");

            storedRefreshToken.UseRefreshToken();

            await _tokenRepository.UpdateToken(storedRefreshToken, cancellationToken);

            var accessToken = _tokenService.GenerateJwtToken(userResult.Value);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Result.Ok(new AccessTokenDto(accessToken.Value, refreshToken.Value));
        }
    }
}