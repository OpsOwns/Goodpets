namespace Goodpets.Application.Abstractions.Security;

public interface ITokenProvider
{
    AccessToken GenerateJwtToken(UserAccount userAccount);
    void ValidatePrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
    bool JwtTokenExpired();
    Guid GetUserIdFromJwtToken();
    bool StoredJwtIdSameAsFromPrinciple(JwtId storedJwtId);
}