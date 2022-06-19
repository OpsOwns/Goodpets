namespace Goodpets.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly GoodpetsContext _goodpetsContext;
    private readonly DbSet<User> _users;

    public UserRepository(GoodpetsContext goodpetsContext)
    {
        _goodpetsContext = goodpetsContext ?? throw new ArgumentNullException(nameof(goodpetsContext));
        _users = _goodpetsContext.Users;
    }

    public async Task UpdateUser(User user, CancellationToken cancellationToken)
    {
        _users.Update(user);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserTransactional(Func<Task> action, User user, CancellationToken cancellationToken)
    {
        await using var transaction = await _goodpetsContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _users.Update(user);
            await _goodpetsContext.SaveChangesAsync(cancellationToken);

            await action();

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
            throw new UserException(nameof(email), "email can not be null or empty");

        var user = await _users.SingleOrDefaultAsync(x => x.Email == email,
            cancellationToken);

        return user ?? null;
    }

    public async Task<User?> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        if (Guid.Empty == userId)
            throw new UserException(nameof(userId), "userId can not be  empty");

        var user = await _users.SingleOrDefaultAsync(x => x.Id == userId,
            cancellationToken);

        return user ?? null;
    }

    public async Task<User?> GetUser(string userName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userName))
            throw new UserException(nameof(userName), "username can not be null or empty");

        var user = await _users.SingleOrDefaultAsync(x => x.Username == userName,
            cancellationToken);

        return user ?? null;
    }

    public async Task<bool> DoesUserEmailExists(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
            throw new UserException(nameof(email), "email can not be null or empty");

        return await _users.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        await _users.AddAsync(user, cancellationToken);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

}