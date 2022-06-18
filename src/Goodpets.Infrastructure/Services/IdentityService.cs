namespace Goodpets.Infrastructure.Services;

internal class UserService : IUserService, IService
{
    private readonly IIdentity _identity;
    private readonly GoodpetsContext _goodpetsContext;
    private readonly DbSet<Token> _tokens;
    private readonly DbSet<User> _users;
    private readonly ITokenProvider _tokenProvider;
    private readonly IClock _clock;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private string Not_Empty(string value) => $"field {value} can't be null or empty";

    public UserService(IIdentity identity, GoodpetsContext goodpetsContext, ITokenProvider tokenProvider, IClock clock,
        IPasswordEncryptor passwordEncryptor)
    {
        _identity = identity;
        _goodpetsContext = goodpetsContext;
        _tokenProvider = tokenProvider;
        _clock = clock;
        _passwordEncryptor = passwordEncryptor;

        _tokens = goodpetsContext.Tokens;
        _users = goodpetsContext.Users;
    }

    public async Task<Result<JsonWebToken>> SignIn(string username, string password,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Fail(Not_Empty(nameof(username)));

        if (string.IsNullOrWhiteSpace(password))
            return Result.Fail(Not_Empty(nameof(password)));

        var user = await _users.SingleOrDefaultAsync(x => x.Username == username,
            cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not exists"));

        var passwordValid =
            _passwordEncryptor.Validate(password, user.Password);

        if (!passwordValid)
            return Result.Fail("Invalid password");

        var accessToken = _tokenProvider.GenerateJwtToken(user);

        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var storedToken = await _tokens
            .SingleOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

        if (storedToken is not null)
        {
            _tokens.Remove(storedToken);
        }

        await _tokens.AddAsync(new Token
        {
            CreationDate = _clock.Current(), Used = false, User = user, ExpireDate = refreshToken.ExpireTime,
            JwtId = accessToken.JwtId.Value, RefreshToken = refreshToken.Value, Id = Guid.NewGuid(),
        }, cancellationToken);


        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(new JsonWebToken(accessToken.Value, refreshToken.Value));
    }


    public async Task<Result> SignUp(string username, string password, string email, string role,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Fail(Not_Empty(nameof(username)));

        if (string.IsNullOrWhiteSpace(password))
            return Result.Fail(Not_Empty(nameof(password)));

        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(Not_Empty(nameof(email)));

        if (string.IsNullOrWhiteSpace(role))
            return Result.Fail(Not_Empty(nameof(role)));

        if (await _goodpetsContext.Users.AnyAsync(x => x.Email == email, cancellationToken))
            return Result.Fail($"User with email {email} already exists in system");

        await _users.AddAsync(new User
        {
            Username = username, Email = email, Id = Guid.NewGuid(), Role = role,
            Password = _passwordEncryptor.Encrypt(password)
        }, cancellationToken);

        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }


    public async Task<Result<JsonWebToken>> RefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            return Result.Fail(Not_Empty(nameof(accessToken)));

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Fail(Not_Empty(nameof(refreshToken)));

        _tokenProvider.ValidatePrincipalFromExpiredToken(accessToken);

        var userId = _tokenProvider.GetUserIdFromJwtToken();

        var user = await _goodpetsContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return Result.Fail("This user not exists");

        var storedRefreshToken =
            await _tokens.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

        if (storedRefreshToken is null)
            return Result.Fail("This refresh token not exists");

        if (_clock.Current() > storedRefreshToken.ExpireDate)
            return Result.Fail("This refresh token has expired");

        if (storedRefreshToken.Used)
            return Result.Fail("This refresh token has been used");


        if (!_tokenProvider.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
            return Result.Fail("This refresh token does not match this JWT");

        storedRefreshToken.Used = true;

        _tokens.Update(storedRefreshToken);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        var jwtToken = _tokenProvider.GenerateJwtToken(user);
        var refreshTokenNew = _tokenProvider.GenerateRefreshToken();

        return Result.Ok(new JsonWebToken(jwtToken.Value, refreshTokenNew.Value));
    }


    public async Task SignOut(CancellationToken cancellationToken)
    {
        var token = await _tokens.SingleAsync(x => x.UserId == _identity.UserAccountId.Value,
            cancellationToken);

        _goodpetsContext.Tokens.Remove(token);

        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }
}