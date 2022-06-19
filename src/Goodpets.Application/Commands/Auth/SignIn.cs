namespace Goodpets.Application.Commands.Auth;

public record SignIn(string UserName, string Password) : ICommand;
