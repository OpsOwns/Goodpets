namespace Goodpets.Infrastructure.Security;

public record RefreshToken(string Value, DateTime ExpireTime);