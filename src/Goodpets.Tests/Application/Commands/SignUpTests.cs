namespace Goodpets.Tests.Application.Commands;

public class SignUpTests
{
    private readonly ICommandHandler<SignUp> _commandHandler;
    private readonly CancellationToken _cancellation;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;


    public SignUpTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _cancellation = new CancellationToken();
        _commandHandler = new SignUpHandler(_userRepository, _passwordManager);
    }

    [Fact]
    public async Task Signup_invalid_when_email_exists()
    {
        var faker = new Faker();
        var signUp = new SignUp(faker.Internet.UserName(), faker.Internet.Password(), faker.Internet.Email(), "user");

        _userRepository.DoesUserEmailExists(Email.Create(signUp.Email).Value, _cancellation).Returns(true);

        var result = await _commandHandler.HandleAsync(signUp, _cancellation);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo($"User with email {signUp.Email} already exists in system");
    }

    [Fact]
    public async Task Signup_process_success()
    {
        var faker = new Faker();
        var signUp = new SignUp(faker.Internet.UserName(), faker.Internet.Password(), faker.Internet.Email(), "user");

        _userRepository.DoesUserEmailExists(Arg.Any<Email>(), _cancellation).Returns(false);
        _passwordManager.Encrypt(signUp.Password).Returns(faker.Internet.Password());

        _userRepository.CreateUser(Arg.Any<User>(), _cancellation).Returns(Task.CompletedTask);

        var result = await _commandHandler.HandleAsync(signUp, _cancellation);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailed.ShouldBeFalse();
    }
}