namespace Goodpets.Infrastructure.Abstractions;

internal interface IUserRepository
{
    Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<User?> GetUser(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetUser(string userName, CancellationToken cancellationToken);
    Task<bool> DoesUserEmailExists(string email, CancellationToken cancellationToken);
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task UpdateUser(User user, CancellationToken cancellationToken);
    Task UpdateUserTransactional(Func<Task> action, User user, CancellationToken cancellationToken);
}