namespace Goodpets.Application.Commands.Handlers;

public class AuthHandler : ICommandHandler<SignUp>, ICommandHandler<SignIn>, ICommandHandler<RefreshToken>,
    ICommandHandler<ChangePassword>, ICommandHandler<ForgotPassword>, ICommandHandler<SignOut>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IIdentity _identity;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IClock _clock;

    public AuthHandler(IEmailService emailService, IUserRepository userRepository,
        IIdentity identity, IPasswordManager passwordManager, ITokenProvider tokenProvider, IClock clock)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _identity = identity;
        _passwordManager = passwordManager;
        _tokenProvider = tokenProvider;
        _clock = clock;
    }

    public async Task<Result> HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.Email);
        var password = Password.Create(command.Password);
        var role = Role.Create(command.Role);
        var username = Username.Create(command.Username);

        var mergedResult = Result.Merge(email, password, role, username);

        if (mergedResult.IsFailed)
        {
            return mergedResult;
        }

        if (await _userRepository.DoesUserEmailExists(email.Value, cancellationToken))
            return Result.Fail(
                new Error($"User with email {email} already exists in system").WithErrorCode("email"));

        var encryptedPassword = Password.Create(_passwordManager.Encrypt(password.Value)).Value;

        var user = new User(role.Value, username.Value, encryptedPassword, email.Value);

        await _userRepository.CreateUser(user, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        var username = Username.Create(command.UserName);
        var password = Password.Create(command.Password);

        var mergeResult = Result.Merge(username, password);

        if (mergeResult.IsFailed)
        {
            return mergeResult;
        }

        var user = await _userRepository.GetUser(username.Value, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not exists").WithErrorCode("user"));

        var passwordValid =
            _passwordManager.Validate(password.Value, user.Password);

        if (!passwordValid)
            return Result.Fail(new Error("Invalid password").WithErrorCode("password"));

        var accessToken = _tokenProvider.GenerateJwtToken(user);

        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var token = Token.Create(refreshToken.Value, refreshToken.ExpireTime, _clock.Current(), false,
            accessToken.JwtId);

        if (token.IsFailed)
            return token.ToResult();

        user.ChangeToken(token.Value);

        await _userRepository.UpdateUser(user);

        _identity.Set(new JsonWebToken(accessToken.Value, refreshToken.Value));

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(RefreshToken command, CancellationToken cancellationToken = default)
    {
        _tokenProvider.ValidatePrincipalFromExpiredToken(command.AccessToken);

        var userId = _tokenProvider.GetUserIdFromJwtToken();

        var user = await _userRepository.GetUser(userId, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("This user not exists").WithErrorCode("user"));

        var storedRefreshToken = await _userRepository.GetRefreshToken(command.Token, cancellationToken);

        if (storedRefreshToken is null)
            return Result.Fail(new Error("This refresh token not exists").WithErrorCode("refreshToken"));

        if (_clock.Current() > storedRefreshToken.ExpireDate)
            return Result.Fail(new Error("This refresh token has expired").WithErrorCode("refreshToken"));

        if (storedRefreshToken.Used)
            return Result.Fail(new Error("This refresh token has been used").WithErrorCode("refreshToken"));


        if (!_tokenProvider.StoredJwtIdSameAsFromPrinciple(storedRefreshToken.JwtId))
            return Result.Fail(new Error("This refresh token does not match this JWT").WithErrorCode("refreshToken"));

        storedRefreshToken = Token.Create(storedRefreshToken.RefreshToken, storedRefreshToken.ExpireDate,
            storedRefreshToken.CreationDate, true, storedRefreshToken.JwtId).Value;

        user.ChangeToken(storedRefreshToken);

        await _userRepository.UpdateUser(user);

        var jwtToken = _tokenProvider.GenerateJwtToken(user);
        var refreshTokenNew = _tokenProvider.GenerateRefreshToken();

        _identity.Set(new JsonWebToken(jwtToken.Value, refreshTokenNew.Value));

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(ChangePassword command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var oldPassword = Password.Create(command.OldPassword);
        var newPassword = Password.Create(command.NewPassword);

        var result = Result.Merge(oldPassword, newPassword);

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var user = await _userRepository.GetUser(_identity.UserId, cancellationToken);

        if (user is null)
            return Result.Fail(new Error("User not found in the system").WithErrorCode("User"));


        if (!_passwordManager.Validate(oldPassword.Value, user.Password.Value))
        {
            return Result.Fail(new Error("Old password is invalid").WithErrorCode("oldPassword"));
        }

        var encryptPassword = _passwordManager.Encrypt(newPassword.Value);

        user.ChangePassword(Password.Create(encryptPassword).Value);

        await _userRepository.UpdateUser(user);

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(ForgotPassword command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var email = Email.Create(command.Email);

        if (email.IsFailed)
            return email.ToResult();

        var user = await _userRepository.GetUserByEmail(email.Value, cancellationToken);

        if (user is null)
            return Result.Fail(new Error($"User with email {email.Value.Value} not exists").WithErrorCode("email"));

        var password = _passwordManager.GeneratePassword(12);

        user.ChangePassword(Password.Create(_passwordManager.Encrypt(password)).Value);

        await _userRepository.UpdateUser(user);

        await _emailService.Send(new EmailMessage(password, command.Email, "[GoodPets] New password"),
            cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(SignOut command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUser(_identity.UserId, cancellationToken);

        if (user?.Token is null)
            return Result.Ok();

        user.RemoveToken();

        await _userRepository.UpdateUser(user);

        return Result.Ok();
    }
}