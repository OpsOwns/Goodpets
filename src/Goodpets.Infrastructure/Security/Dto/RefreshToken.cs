namespace Goodpets.Infrastructure.Security.Dto;

public record RefreshToken(string Value, DateTime ExpireTime);