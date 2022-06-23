namespace Goodpets.Application.User.DTO;

public record RefreshTokenDto(string Value, LocalDateTime ExpireTime);

public record JsonWebToken(string AccessToken, string RefreshToken);

public record AccessTokenDto(string Value, JwtId JwtId);