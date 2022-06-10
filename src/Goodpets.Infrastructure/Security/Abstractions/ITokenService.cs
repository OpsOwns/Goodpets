namespace Goodpets.Infrastructure.Security.Abstractions;

public interface ITokenService
{
    DateTime ExpireDateToken { get; }
    string GenerateJwtToken(UserAccount userAccount);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GenerateRefreshToken();
}