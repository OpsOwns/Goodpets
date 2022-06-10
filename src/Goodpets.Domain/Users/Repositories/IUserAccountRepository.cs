using Goodpets.Shared.Abstractions;

namespace Goodpets.Domain.Users.Repositories;

public interface IUserAccountRepository : IRepository
{
    Task<bool> CheckUserExistsByEmail(Email email, CancellationToken cancellationToken);
    Task Add(UserAccount userAccount, CancellationToken cancellationToken);
    Task<UserAccount> GetUserAccount(UserAccountId userAccountId, CancellationToken cancellationToken);
    Task<UserAccount> GetUserAccount(string username, CancellationToken cancellationToken);
    Task<UserAccount> GetUserByToken(string refreshToken, CancellationToken cancellationToken);
    Task UpdateUser(UserAccount userAccount, CancellationToken cancellationToken);
}