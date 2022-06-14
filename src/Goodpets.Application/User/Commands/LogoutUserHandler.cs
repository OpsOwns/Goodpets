namespace Goodpets.Application.User.Commands;

public record LogoutUser : ICommand;

public class LogoutUserHandler : ICommandHandler<LogoutUser>
{
    private readonly IIdentity _identity;
    private readonly ITokenRepository _tokenRepository;

    public LogoutUserHandler(IIdentity identity, ITokenRepository tokenRepository)
    {
        _identity = identity;
        _tokenRepository = tokenRepository;
    }

    public async Task HandleAsync(LogoutUser command, CancellationToken cancellationToken = default)
    {
        var token = await _tokenRepository.GetToken(_identity.UserAccountId, cancellationToken);

        if (token.IsFailed)
            return;

        await _tokenRepository.RemoveToken(token.Value, cancellationToken);
    }
}