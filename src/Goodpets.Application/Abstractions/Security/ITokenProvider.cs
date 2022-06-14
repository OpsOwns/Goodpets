namespace Goodpets.Application.Abstractions.Security;

public interface ITokenProvider
{
    AccessToken GenerateJwtToken(UserAccount userAccount);
    void ValidatePrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
    Guid GetUserIdFromJwtToken();
    bool StoredJwtIdSameAsFromPrinciple(JwtId storedJwtId);
}