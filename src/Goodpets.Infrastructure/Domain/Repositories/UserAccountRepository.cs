namespace Goodpets.Infrastructure.Domain.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly DbSet<UserAccount> _accounts;
    private readonly GoodpetsContext _goodpetsContext;

    public UserAccountRepository(GoodpetsContext goodpetsContext)
    {
        _goodpetsContext = goodpetsContext;
        _accounts = _goodpetsContext.UserAccount;
    }

    public async Task<bool> CheckUserExistsByEmail(Email email, CancellationToken cancellationToken = default)
    {
        return await _goodpetsContext.UserAccount.AnyAsync(x => x.Email.Value == email.Value, cancellationToken);
    }

    public async Task Add(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        await _goodpetsContext.UserAccount.AddAsync(userAccount, cancellationToken);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<UserAccount>> GetUserAccount(UserAccountId userAccountId,
        CancellationToken cancellationToken = default)
    {
        var user = await _goodpetsContext.UserAccount.FindAsync(new object[] { userAccountId }, cancellationToken);

        if (user is null) return Result.Fail(CustomResultMessage.Not_Found(nameof(user)));

        return Result.Ok(user);
    }

    public async Task<Result<UserAccount>> GetUserAccount(string username,
        CancellationToken cancellationToken = default)
    {
        var user = await _accounts.SingleOrDefaultAsync(x => x.Credentials.Username == username,
            cancellationToken);

        if (user is null) return Result.Fail(CustomResultMessage.Not_Found(nameof(user)));

        return Result.Ok(user);
    }
}