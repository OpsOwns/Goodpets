namespace Goodpets.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(GoodpetsContext goodpetsContext)
    {
        if (goodpetsContext == null)
            throw new ArgumentNullException(nameof(goodpetsContext));

        _users = goodpetsContext.Users;
    }

    public Task UpdateUser(User user)
    {
        _users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<Token?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new UserException(nameof(refreshToken), "refreshToken can not be null or empty");

        var user = await _users.SingleOrDefaultAsync(x => x.Token.RefreshToken == refreshToken,
            cancellationToken);

        return user?.Token ?? null;
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

    public async Task CreateUser(User user, CancellationToken cancellationToken) =>
        await _users.AddAsync(user, cancellationToken);
}