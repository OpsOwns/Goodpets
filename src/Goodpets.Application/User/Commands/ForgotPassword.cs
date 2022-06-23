namespace Goodpets.Application.User.Commands;

public record ForgotPassword(string Email) : ICommand;