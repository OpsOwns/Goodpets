namespace Goodpets.Domain.Users.Repositories;

public interface IUserAccountRepository : IRepository
{
    Task<bool> CheckUserExistsByEmail(Email email, CancellationToken cancellationToken);
    Task Add(UserAccount userAccount, CancellationToken cancellationToken);
    Task<Result<UserAccount>> GetUserAccount(UserAccountId userAccountId, CancellationToken cancellationToken);
    Task<Result<UserAccount>> GetUserAccount(string username, CancellationToken cancellationToken);
}