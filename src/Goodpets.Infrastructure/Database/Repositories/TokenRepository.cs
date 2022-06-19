namespace Goodpets.Infrastructure.Database.Repositories;

internal class TokenRepository : ITokenRepository
{
    private readonly GoodpetsContext _goodpetsContext;
    private readonly DbSet<Token> _tokens;

    public TokenRepository(GoodpetsContext goodpetsContext)
    {
        _goodpetsContext = goodpetsContext;
        _tokens = _goodpetsContext.Tokens;
    }

    public async Task ReplaceRefreshToken(Guid userId, Token token, CancellationToken cancellationToken)
    {
        var storedToken = await _tokens
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (storedToken is not null)
        {
            _tokens.Remove(storedToken);
        }

        await _tokens.AddAsync(token, cancellationToken);


        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRefreshToken(Token token, CancellationToken cancellationToken)
    {
        _tokens.Update(token);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Token?> GetToken(Guid userId, CancellationToken cancellationToken)
    {
        if (Guid.Empty == userId)
            throw new UserException(nameof(userId), "userId can not be empty");

        var token = await _tokens.SingleOrDefaultAsync(x => x.UserId == userId,
            cancellationToken);

        return token ?? null;
    }

    public async Task RemoveToken(Token token, CancellationToken cancellationToken)
    {
        _goodpetsContext.Tokens.Remove(token);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
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