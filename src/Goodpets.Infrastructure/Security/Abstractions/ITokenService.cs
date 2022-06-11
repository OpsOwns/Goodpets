namespace Goodpets.Infrastructure.Security.Abstractions;

public interface ITokenService
{
    string GenerateJwtToken(UserAccount userAccount);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
}