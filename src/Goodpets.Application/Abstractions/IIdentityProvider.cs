namespace Goodpets.Application.Abstractions;

public interface IIdentityProvider
{
    Task<Result> SignIn(string username, string password,
        CancellationToken cancellationToken);

    Task<Result> SignUp(string username, string password, string email, string role,
        CancellationToken cancellationToken);

    Task<Result> RefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken);

    Task SignOut(CancellationToken cancellationToken);

    Task<Result> ChangePassword(string newPassword, string oldPassword, CancellationToken cancellationToken);
    Task<Result<string>> ResetPassword(string email, CancellationToken cancellationToken);
}