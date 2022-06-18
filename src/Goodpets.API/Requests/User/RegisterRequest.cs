namespace Goodpets.API.Requests.User;

public record RegisterRequest(string Email, string UserName, string Password, string Role);