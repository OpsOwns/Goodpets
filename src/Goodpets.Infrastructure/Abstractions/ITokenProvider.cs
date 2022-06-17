namespace Goodpets.Infrastructure.Abstractions;

internal interface ITokenProvider
{
    AccessToken GenerateJwtToken(User user);
    void ValidatePrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
    Guid GetUserIdFromJwtToken();
    bool StoredJwtIdSameAsFromPrinciple(Guid storedJwtId);
}