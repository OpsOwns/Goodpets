namespace Goodpets.API.Requests.User;

public record SignUpRequest(string Email, string UserName, string Password, string Role);