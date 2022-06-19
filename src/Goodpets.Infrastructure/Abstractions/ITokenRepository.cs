namespace Goodpets.Infrastructure.Abstractions;

internal interface ITokenRepository : IRepository
{
    Task ReplaceRefreshToken(Token token, CancellationToken cancellationToken);
    Task UpdateRefreshToken(Token token);
    Task<Token?> GetToken(Guid userId, CancellationToken cancellationToken);
    Task RemoveToken(Token token);
    Task<Token?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken);
}