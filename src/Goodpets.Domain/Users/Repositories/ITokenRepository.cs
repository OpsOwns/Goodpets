namespace Goodpets.Domain.Users.Repositories;

public interface ITokenRepository : IRepository
{
    Task<Result<Token>> GetToken(string refreshToken, CancellationToken cancellationToken);
    Task<Result<Token>> GetToken(UserAccountId userAccountId, CancellationToken cancellationToken);
    Task Create(Token token, CancellationToken cancellationToken);
    Task RemoveToken(Token token, CancellationToken cancellationToken);
    Task UpdateToken(Token token, CancellationToken cancellationToken);
}