namespace Goodpets.Infrastructure.Database.Repositories;

internal class TokenRepository : ITokenRepository
{
    private readonly DbSet<Token> _tokens;

    public TokenRepository(GoodpetsContext goodpetsContext)
    {
        _tokens = goodpetsContext.Tokens;
    }

    public async Task ReplaceRefreshToken(Token token, CancellationToken cancellationToken)
    {
        var storedToken = await _tokens
            .SingleOrDefaultAsync(x => x.UserId == token.User.Id, cancellationToken);

        if (storedToken is not null)
        {
            _tokens.Remove(storedToken);
        }

        _tokens.Attach(token);

        await _tokens.AddAsync(token, cancellationToken);
    }

    public Task UpdateRefreshToken(Token token, CancellationToken cancellationToken)
    {
        _tokens.Update(token);
        return Task.CompletedTask;
    }

    public async Task<Token?> GetToken(Guid userId, CancellationToken cancellationToken)
    {
        if (Guid.Empty == userId)
            throw new UserException(nameof(userId), "userId can not be empty");

        var token = await _tokens.SingleOrDefaultAsync(x => x.UserId == userId,
            cancellationToken);

        return token ?? null;
    }

    public Task RemoveToken(Token token, CancellationToken cancellationToken)
    {
        _tokens.Remove(token);
        return Task.CompletedTask;
    }

    public async Task<Token?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new UserException(nameof(refreshToken), "refreshToken can not be null or empty");

        var token = await _tokens.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken,
            cancellationToken);

        return token;
    }
}