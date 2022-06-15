using Goodpets.Infrastructure.Security.Models;

namespace Goodpets.Infrastructure.Abstractions;

public interface ITokenProvider
{
    AccessToken GenerateJwtToken(User user);
    void ValidatePrincipalFromExpiredToken(string token);
    RefreshToken GenerateRefreshToken();
    Guid GetUserIdFromJwtToken();
    bool StoredJwtIdSameAsFromPrinciple(Guid storedJwtId);
}