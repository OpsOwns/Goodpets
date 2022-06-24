namespace Goodpets.Tests.Application.Commands;

public class SignInTests
{
    private readonly ICommandHandler<SignIn> _commandHandler;
    private readonly CancellationToken _cancellation;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentity _identity;

    public SignInTests()
    {
        _cancellation = new CancellationToken();
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _tokenProvider = Substitute.For<ITokenProvider>();
        _identity = Substitute.For<IIdentity>();


        _commandHandler = new SignInHandler(_userRepository, _passwordManager, _tokenProvider, _identity);
    }

    [Fact]
    public async Task SigIn_Invalid_User_Not_Exists()
    {
        var faker = new Faker();
        var signIn = new SignIn(faker.Internet.UserName(), faker.Internet.Password());

        _userRepository.GetUser(Username.Create(signIn.UserName).Value, _cancellation)
            .Returns(Task.FromResult<User?>(null));

        var result = await _commandHandler.HandleAsync(signIn, _cancellation);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("User not exists");
    }

    [Fact]
    public async Task SigIn_Invalid_Password()
    {
        var faker = new Faker();
        var signIn = new SignIn(faker.Internet.UserName(), faker.Internet.Password());

        _userRepository.GetUser(Username.Create(signIn.UserName).Value, _cancellation).Returns(Task.FromResult<User?>
            (FakeUser.CreateFakeUser()));

        _passwordManager.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        var result = await _commandHandler.HandleAsync(signIn, _cancellation);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("Invalid password");
    }

    [Fact]
    public async Task SigIn_Success()
    {
        var faker = new Faker();
        var signIn = new SignIn(faker.Internet.UserName(), faker.Internet.Password());

        _userRepository.GetUser(Username.Create(signIn.UserName).Value, _cancellation).Returns(Task.FromResult<User?>
            (FakeUser.CreateFakeUser()));

        _passwordManager.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        _tokenProvider.GenerateJwtToken(Arg.Any<User>())
            .Returns(new AccessTokenDto(faker.Random.Word(), new JwtId()));
        _tokenProvider.GenerateRefreshToken()
            .Returns(new RefreshTokenDto(faker.Random.Word(), LocalDateTime.MaxIsoValue));

        _userRepository.UpdateUser(Arg.Any<User>()).Returns(Task.CompletedTask);

        _identity.When(x => x.Set(Arg.Any<JsonWebToken>()));

        (await _commandHandler.HandleAsync(signIn, _cancellation)).IsSuccess.ShouldBeTrue();
    }
}