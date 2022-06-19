namespace Goodpets.Application.Commands.Auth;

public record SignUp(string Username, string Password, string Email, string Role) : ICommand;