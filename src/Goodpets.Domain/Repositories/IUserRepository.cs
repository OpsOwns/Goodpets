namespace Goodpets.Domain.Repositories;

public interface IUserRepository : IRepository
{
    Task<User?> GetUserByEmail(Email email, CancellationToken cancellationToken);
    Task<User?> GetUser(UserId userId, CancellationToken cancellationToken);
    Task<User?> GetUser(Username userName, CancellationToken cancellationToken);
    Task<bool> DoesUserEmailExists(Email email, CancellationToken cancellationToken);
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task UpdateUser(User user);
    Task<Token?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken);
}