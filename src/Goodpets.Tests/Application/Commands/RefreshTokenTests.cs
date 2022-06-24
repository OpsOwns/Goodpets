namespace Goodpets.Tests.Application.Commands;

public class RefreshTokenTests
{
    private readonly ICommandHandler<RefreshToken> _commandHandler;
    private readonly ITokenProvider _provider;
    private readonly IIdentity _identity;
    private readonly IUserRepository _userRepository;

    public RefreshTokenTests()
    {
        _provider = Substitute.For<ITokenProvider>();
        _identity = Substitute.For<IIdentity>();
        _userRepository = Substitute.For<IUserRepository>();

        _commandHandler = new RefreshTokenHandler(_provider, _userRepository, _identity);
    }

    [Fact]
    public async Task RefreshToken_Invalid_User_Not_Exists()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None).Returns(Task.FromResult<User?>(null));

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This user not exists");
    }

    [Fact]
    public async Task RefreshToken_Not_Found_StoredToken()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(FakeUser.CreateFakeUser()));

        _userRepository.GetRefreshToken(Arg.Any<string>(), CancellationToken.None)
            .Returns(Task.FromResult<Token?>(null));

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This refresh token not exists");
    }

    [Fact]
    public async Task RefreshToken__Invalid_Refresh_StoredToken_Expired()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(FakeUser.CreateFakeUser()));

        _userRepository.GetRefreshToken(Arg.Any<string>(), CancellationToken.None)
            .Returns(Task.FromResult<Token?>(Token
                .Create(faker.Random.Word(), new LocalDateTime(2022, 1, 1, 0, 0), false, new JwtId()).Value));

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This refresh token has expired");
    }

    [Fact]
    public async Task RefreshToken__Invalid_Refresh_StoredToken_Used()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(FakeUser.CreateFakeUser()));

        _userRepository.GetRefreshToken(Arg.Any<string>(), CancellationToken.None)
            .Returns(Task.FromResult<Token?>(Token
                .Create(faker.Random.Word(),
                    SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime.Plus(Period.FromDays(1)), true,
                    new JwtId()).Value));

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This refresh token has been used");
    }

    [Fact]
    public async Task RefreshToken__Invalid_Saved_StoredToken()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(FakeUser.CreateFakeUser()));

        _userRepository.GetRefreshToken(Arg.Any<string>(), CancellationToken.None)
            .Returns(Task.FromResult<Token?>(Token
                .Create(faker.Random.Word(),
                    SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime.Plus(Period.FromDays(1)), false,
                    new JwtId()).Value));

        _provider.StoredJwtIdSameAsFromPrinciple(new Guid()).Returns(false);

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This refresh token does not match this JWT");
    }

    [Fact]
    public async Task RefreshToken_Success()
    {
        var faker = new Faker();
        var token = new RefreshToken(faker.Random.Word(), faker.Random.Word());

        _provider.When(x => x.ValidatePrincipalFromExpiredToken(Arg.Any<string>()));

        _provider.GetUserIdFromJwtToken().Returns(new UserId());

        var fakeUser = FakeUser.CreateFakeUser();

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(fakeUser));

        var tokenJwt = Token
            .Create(faker.Random.Word(),
                SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime.Plus(Period.FromDays(1)), false,
                new JwtId()).Value;

        _userRepository.GetRefreshToken(Arg.Any<string>(), CancellationToken.None)
            .Returns(Task.FromResult<Token?>(tokenJwt));

        _provider.StoredJwtIdSameAsFromPrinciple(tokenJwt.JwtId).Returns(true);

        _userRepository.UpdateUser(fakeUser).Returns(Task.CompletedTask);

        _provider.GenerateJwtToken(fakeUser)
            .Returns(new AccessTokenDto(faker.Random.Word(), new JwtId()));

        _provider.GenerateRefreshToken().Returns(new RefreshTokenDto(faker.Random.Word(), LocalDateTime.MaxIsoValue));

        _identity.When(x => x.Set(Arg.Any<JsonWebToken>()));

        var result = await _commandHandler.HandleAsync(token, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}