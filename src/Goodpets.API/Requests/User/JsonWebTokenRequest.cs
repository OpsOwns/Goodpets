namespace Goodpets.API.Requests.User;

public record JsonWebTokenRequest(string AccessToken, string RefreshToken);