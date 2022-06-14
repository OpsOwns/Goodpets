namespace Goodpets.Infrastructure.Domain.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly GoodpetsContext _goodpetsContext;
    private readonly DbSet<Token> _tokens;

    public TokenRepository(GoodpetsContext goodpetsContext)
    {
        _goodpetsContext = goodpetsContext;
        _tokens = goodpetsContext.Tokens;
    }

    public async Task<Result<Token>> GetToken(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _tokens.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

        if (token is null) return Result.Fail(CustomResultMessage.Not_Found(nameof(token)));

        return Result.Ok(token);
    }

    public async Task<Result<Token>> GetToken(UserAccountId userAccountId, CancellationToken cancellationToken)
    {
        var token = await _tokens.SingleOrDefaultAsync(x => x.UserAccountId == userAccountId, cancellationToken);

        if (token is null) return Result.Fail(CustomResultMessage.Not_Found(nameof(token)));

        return Result.Ok(token);
    }

    public async Task Create(Token token, CancellationToken cancellationToken)
    {
        await _tokens.AddAsync(token, cancellationToken);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveToken(Token token, CancellationToken cancellationToken)
    {
        _tokens.Remove(token);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateToken(Token token, CancellationToken cancellationToken)
    {
        _tokens.Update(token);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }
}