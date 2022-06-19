namespace Goodpets.Application.Commands.Auth;

public record ForgotPassword(string Email) : ICommand;