namespace Goodpets.Application.Dto;

public record RefreshTokenDto(string Value, DateTime ExpireTime);

public record JsonWebToken(string AccessToken, string RefreshToken);

public record AccessTokenDto(string Value, JwtId JwtId);