namespace Goodpets.Infrastructure.Abstractions;

public interface IUserService
{
    Task<Result<AccessTokenDto>> SignIn(string username, string password,
        CancellationToken cancellationToken);

    Task<Result> SignUp(string username, string password, string email, string role,
        CancellationToken cancellationToken);

    Task<Result<AccessTokenDto>> RefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken);

    Task SignOut(CancellationToken cancellationToken);
}