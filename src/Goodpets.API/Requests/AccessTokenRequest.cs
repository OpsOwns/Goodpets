namespace Goodpets.API.Requests;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);