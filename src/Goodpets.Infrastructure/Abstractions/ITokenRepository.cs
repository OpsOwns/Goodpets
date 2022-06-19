namespace Goodpets.Infrastructure.Abstractions;

internal interface ITokenRepository
{
    Task ReplaceRefreshToken(Token token, CancellationToken cancellationToken);
    Task UpdateRefreshToken(Token token, CancellationToken cancellationToken);
    Task<Token?> GetToken(Guid userId, CancellationToken cancellationToken);
    Task RemoveToken(Token token, CancellationToken cancellationToken);
    Task<Token?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken);
}