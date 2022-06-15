namespace Goodpets.API.Requests;

public record UserRegisterRequest(string Email, string UserName, string Password, string Role);