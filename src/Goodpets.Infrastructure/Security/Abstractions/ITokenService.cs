using Goodpets.Infrastructure.Security.Dto;

namespace Goodpets.Infrastructure.Security.Abstractions;

public interface ITokenService
{
    AccessToken GenerateJwtToken(UserAccount userAccount);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
    bool JwtTokenExpired(long expiryDateUnix);
}