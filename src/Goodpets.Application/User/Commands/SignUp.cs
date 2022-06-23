namespace Goodpets.Application.User.Commands;

public record SignUp(string Username, string Password, string Email, string Role) : ICommand;