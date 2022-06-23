namespace Goodpets.Application.User.Commands;

public record ChangePassword(string OldPassword, string NewPassword) : ICommand;