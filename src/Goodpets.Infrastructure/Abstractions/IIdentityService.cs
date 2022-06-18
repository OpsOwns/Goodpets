namespace Goodpets.Infrastructure.Abstractions;

public interface IIdentityService
{
    Task<Result<JsonWebToken>> SignIn(string username, string password,
        CancellationToken cancellationToken);

    Task<Result> SignUp(string username, string password, string email, string role,
        CancellationToken cancellationToken);

    Task<Result<JsonWebToken>> RefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken);

    Task SignOut(CancellationToken cancellationToken);

    Task<Result> ChangePassword(string newPassword, string oldPassword, CancellationToken cancellationToken);
    Task<Result> ResetPassword(string email, CancellationToken cancellationToken);
}