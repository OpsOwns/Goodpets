﻿namespace Goodpets.Infrastructure.Database.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly GoodpetsContext _goodpetsContext;

    public UserAccountRepository(GoodpetsContext goodpetsContext)
    {
        _goodpetsContext = goodpetsContext;
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

    public async Task<UserAccount> GetUserAccount(UserAccountId userAccountId,
        CancellationToken cancellationToken = default)
    {
        var user = await _goodpetsContext.UserAccount.FindAsync(new object[] { userAccountId }, cancellationToken);
        return user ?? UserAccount.NotFound();
    }

    public async Task<UserAccount> GetUserAccount(string username,
        CancellationToken cancellationToken = default)
    {
        var user = await _goodpetsContext.UserAccount.SingleOrDefaultAsync(x => x.Credentials.Username == username,
            cancellationToken);
        return user ?? UserAccount.NotFound();
    }
}