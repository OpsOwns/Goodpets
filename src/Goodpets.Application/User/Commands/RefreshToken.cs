namespace Goodpets.Application.User.Commands;

public record RefreshToken(string AccessToken, string Token) : ICommand;