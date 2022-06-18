namespace Goodpets.API.Requests;

public record JsonWebTokenRequest(string AccessToken, string RefreshToken);