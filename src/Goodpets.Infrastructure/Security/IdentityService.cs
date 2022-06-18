namespace Goodpets.Infrastructure.Security;

internal class IdentityService : IIdentityService
{
    private readonly IIdentity _identity;
    private readonly GoodpetsContext _goodpetsContext;
    private readonly DbSet<Token> _tokens;
    private readonly DbSet<User> _users;
    private readonly ITokenProvider _tokenProvider;
    private readonly IClock _clock;
    private readonly IPasswordManager _passwordManager;
    private readonly IEmailService _emailService;
    private string Not_Empty(string value) => $"field {value} can't be null or empty";

    public IdentityService(IIdentity identity, GoodpetsContext goodpetsContext, ITokenProvider tokenProvider,
        IClock clock,
        IPasswordManager passwordManager, IEmailService emailService)
    {
        _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        _goodpetsContext = goodpetsContext ?? throw new ArgumentNullException(nameof(goodpetsContext));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

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
            return Result.Fail(new Error("User not exists").WithErrorCode("user"));

        var passwordValid =
            _passwordManager.Validate(password, user.Password);

        if (!passwordValid)
            return Result.Fail(new Error("Invalid password").WithErrorCode("password"));

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
            return Result.Fail(
                new Error($"User with email {email} already exists in system").WithErrorCode("email"));

        await _users.AddAsync(new User
        {
            Username = username, Email = email, Id = Guid.NewGuid(), Role = role,
            Password = _passwordManager.Encrypt(password)
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
            return Result.Fail(new Error("This user not exists").WithErrorCode("user"));

        var storedRefreshToken =
            await _tokens.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

        if (storedRefreshToken is null)
            return Result.Fail(new Error("This refresh token not exists").WithErrorCode("refreshToken"));

        if (_clock.Current() > storedRefreshToken.ExpireDate)
            return Result.Fail(new Error("This refresh token has expired").WithErrorCode("refreshToken"));

        if (storedRefreshToken.Used)
            return Result.Fail(new Error("This refresh token has been used").WithErrorCode("refreshToken"));


        if (!_tokenProvider.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
            return Result.Fail(new Error("This refresh token does not match this JWT").WithErrorCode("refreshToken"));

        storedRefreshToken.Used = true;

        _tokens.Update(storedRefreshToken);
        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        var jwtToken = _tokenProvider.GenerateJwtToken(user);
        var refreshTokenNew = _tokenProvider.GenerateRefreshToken();

        return Result.Ok(new JsonWebToken(jwtToken.Value, refreshTokenNew.Value));
    }


    public async Task SignOut(CancellationToken cancellationToken)
    {
        var token = await _tokens.SingleOrDefaultAsync(x => x.UserId == _identity.UserAccountId.Value,
            cancellationToken);

        if (token is null)
            return;

        _goodpetsContext.Tokens.Remove(token);

        await _goodpetsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> ChangePassword(string newPassword, string oldPassword,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            return Result.Fail(Not_Empty(nameof(newPassword)));

        if (string.IsNullOrWhiteSpace(oldPassword))
            return Result.Fail(Not_Empty(nameof(oldPassword)));

        var user = await _users.SingleOrDefaultAsync(x => x.Id == _identity.UserAccountId.Value, cancellationToken);

        if (user is null)
            throw new UserException("system can't find user in database");

        if (!_passwordManager.Validate(oldPassword, user.Password))
        {
            return Result.Fail(new Error("Old password is invalid").WithErrorCode("oldPassword"));
        }

        user.Password = _passwordManager.Encrypt(newPassword);

        _users.Attach(user);

        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> ResetPassword(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(Not_Empty(nameof(email)));

        var user = await _users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
            return Result.Fail(new Error($"User with email {email} not exists").WithErrorCode("email"));

        var password = _passwordManager.GeneratePassword(12);

        user.Password = _passwordManager.Encrypt(password);

        _users.Attach(user);

        await _emailService.Send(new EmailMessage(password, user.Email, "[GoodPets] New password"), cancellationToken);

        await _goodpetsContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}