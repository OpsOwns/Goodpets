namespace Goodpets.Application.Abstractions.Security;

public interface ITokenProvider
{
    AccessTokenDto GenerateJwtToken(Domain.Entities.User user);
    void ValidatePrincipalFromExpiredToken(string token);
    RefreshTokenDto GenerateRefreshToken();
    UserId GetUserIdFromJwtToken();
    bool StoredJwtIdSameAsFromPrinciple(Guid storedJwtId);
}