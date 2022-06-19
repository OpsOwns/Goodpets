namespace Goodpets.Application.Commands.Auth;

public record RefreshToken(string AccessToken, string Token) : ICommand;