namespace Goodpets.Infrastructure.Security.Auth;

internal class IdentityProvider : IIdentityProvider
{
    private readonly IIdentity _identity;
    private readonly ITokenProvider _tokenProvider;
    private readonly IClock _clock;
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private string Not_Empty(string value) => $"field {value} can't be null or empty";

    public IdentityProvider(IIdentity identity, ITokenProvider tokenProvider, IClock clock,
        IPasswordManager passwordManager, IUserRepository userRepository)
    {
        _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result> SignIn(string username, string password,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Fail(Not_Empty(nameof(username)));

        if (string.IsNullOrWhiteSpace(password))
            return Result.Fail(Not_Empty(nameof(password)));

        var user = await _userRepository.GetUser(username, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not exists").WithErrorCode("user"));

        var passwordValid =
            _passwordManager.Validate(password, user.Password);

        if (!passwordValid)
            return Result.Fail(new Error("Invalid password").WithErrorCode("password"));

        var accessToken = _tokenProvider.GenerateJwtToken(user);

        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var token = new Token
        {
            CreationDate = _clock.Current(), Used = false, ExpireDate = refreshToken.ExpireTime,
            JwtId = accessToken.JwtId.Value, RefreshToken = refreshToken.Value,
        };

        user.Token = token;

        await _userRepository.UpdateUser(user);

        _identity.Set(new JsonWebToken(accessToken.Value, refreshToken.Value));

        return Result.Ok();
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

        if (await _userRepository.DoesUserEmailExists(email, cancellationToken))
            return Result.Fail(
                new Error($"User with email {email} already exists in system").WithErrorCode("email"));

        await _userRepository.CreateUser(new User
        {
            Username = username, Email = email, Id = Guid.NewGuid(), Role = role,
            Password = _passwordManager.Encrypt(password)
        }, cancellationToken);

        return Result.Ok();
    }


    public async Task<Result> RefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            return Result.Fail(Not_Empty(nameof(accessToken)));

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Fail(Not_Empty(nameof(refreshToken)));

        _tokenProvider.ValidatePrincipalFromExpiredToken(accessToken);

        var userId = _tokenProvider.GetUserIdFromJwtToken();

        var user = await _userRepository.GetUser(userId, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("This user not exists").WithErrorCode("user"));

        var storedRefreshToken = await _userRepository.GetRefreshToken(refreshToken, cancellationToken);

        if (storedRefreshToken is null)
            return Result.Fail(new Error("This refresh token not exists").WithErrorCode("refreshToken"));

        if (_clock.Current() > storedRefreshToken.ExpireDate)
            return Result.Fail(new Error("This refresh token has expired").WithErrorCode("refreshToken"));

        if (storedRefreshToken.Used)
            return Result.Fail(new Error("This refresh token has been used").WithErrorCode("refreshToken"));


        if (!_tokenProvider.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
            return Result.Fail(new Error("This refresh token does not match this JWT").WithErrorCode("refreshToken"));

        storedRefreshToken.Used = true;

        user.Token = storedRefreshToken;

        await _userRepository.UpdateUser(user);

        var jwtToken = _tokenProvider.GenerateJwtToken(user);
        var refreshTokenNew = _tokenProvider.GenerateRefreshToken();

        _identity.Set(new JsonWebToken(jwtToken.Value, refreshTokenNew.Value));

        return Result.Ok();
    }


    public async Task SignOut(CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(_identity.UserId.Value, cancellationToken);

        if (user is null)
            return;

        user.Token = null;

        await _userRepository.UpdateUser(user);
    }

    public async Task<Result> ChangePassword(string newPassword, string oldPassword,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            return Result.Fail(Not_Empty(nameof(newPassword)));

        if (string.IsNullOrWhiteSpace(oldPassword))
            return Result.Fail(Not_Empty(nameof(oldPassword)));

        var user = await _userRepository.GetUser(_identity.UserId.Value, cancellationToken);

        if (user is null)
            throw new UserException("system can't find user in database");

        if (!_passwordManager.Validate(oldPassword, user.Password))
        {
            return Result.Fail(new Error("Old password is invalid").WithErrorCode("oldPassword"));
        }

        user.Password = _passwordManager.Encrypt(newPassword);

        await _userRepository.UpdateUser(user);

        return Result.Ok();
    }

    public async Task<Result<string>> ResetPassword(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(Not_Empty(nameof(email)));

        var user = await _userRepository.GetUserByEmail(email, cancellationToken);

        if (user is null)
            return Result.Fail(new Error($"User with email {email} not exists").WithErrorCode("email"));

        var password = _passwordManager.GeneratePassword(12);

        user.Password = _passwordManager.Encrypt(password);

        await _userRepository.UpdateUser(user);

        return Result.Ok(password);
    }
}