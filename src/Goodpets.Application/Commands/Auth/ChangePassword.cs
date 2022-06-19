namespace Goodpets.Application.Commands.Auth;

public record ChangePassword(string OldPassword, string NewPassword) : ICommand;