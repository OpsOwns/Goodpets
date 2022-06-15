namespace Goodpets.Application.User.Commands;

public record SignOut : ICommand;

public class SignOutHandler : ICommandHandler<SignOut>
{
    private readonly IIdentity _identity;
    private readonly ITokenRepository _tokenRepository;

    public SignOutHandler(IIdentity identity, ITokenRepository tokenRepository)
    {
        _identity = identity;
        _tokenRepository = tokenRepository;
    }

    public async Task HandleAsync(SignOut command, CancellationToken cancellationToken = default)
    {
        var token = await _tokenRepository.GetToken(_identity.UserAccountId, cancellationToken);

        if (token.IsFailed)
            return;

        await _tokenRepository.RemoveToken(token.Value, cancellationToken);
    }
}