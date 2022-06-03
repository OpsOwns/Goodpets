using Goodpets.Shared.Abstractions;

namespace Goodpets.Domain.Users.Repositories;

public interface IUserAccountRepository : IRepository
{
    public Task<bool> CheckUserExistsByEmail(Email email, CancellationToken cancellationToken = default);
    public Task Add(UserAccount userAccount, CancellationToken cancellationToken = default);
    public Task<UserAccount> GetUserAccount(UserAccountId userAccountId, CancellationToken cancellationToken = default);
    public Task<UserAccount> GetUserAccount(string username, CancellationToken cancellationToken = default);
}