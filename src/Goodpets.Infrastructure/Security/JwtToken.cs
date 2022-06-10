namespace Goodpets.Infrastructure.Security;

public record JwtToken(string AccessToken, string RefreshToken, DateTime ExpireRefreshToken);