namespace Goodpets.Application.User.Commands;

public record SignIn(string UserName, string Password) : ICommand;
