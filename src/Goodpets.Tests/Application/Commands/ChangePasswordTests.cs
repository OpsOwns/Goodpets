namespace Goodpets.Tests.Application.Commands;

public class ChangePasswordTests
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentity _identity;
    private readonly IPasswordManager _passwordManager;
    private readonly ICommandHandler<ChangePassword> _commandHandler;

    public ChangePasswordTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _identity = Substitute.For<IIdentity>();
        _passwordManager = Substitute.For<IPasswordManager>();

        _commandHandler = new ChangePasswordHandler(_userRepository, _identity, _passwordManager);
    }

    [Fact]
    public async Task ChangePassword_Not_Found_User()
    {
        var faker = new Faker();
        var command = new ChangePassword(faker.Internet.Password(), faker.Internet.Password());

        var result = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command, CancellationToken.None));

        result.ShouldBeOfType<NotFoundException>();
        result.Message.ShouldBeEquivalentTo("System unable to find value");
    }

    [Fact]
    public async Task ChangePassword_Invalid_OldPassword()
    {
        var faker = new Faker();
        var command = new ChangePassword(faker.Internet.Password(), faker.Internet.Password());
        var user = FakeUser.CreateFakeUser();


        _userRepository.GetUser(user.UserId, CancellationToken.None).Returns(user);

        _identity.UserId.Returns(user.UserId);

        _passwordManager.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        var result = await _commandHandler.HandleAsync(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("Old password is invalid");
    }

    [Fact]
    public async Task ChangePassword_ToNew_Success()
    {
        var faker = new Faker();
        var command = new ChangePassword(faker.Internet.Password(), faker.Internet.Password());
        var user = FakeUser.CreateFakeUser();


        _userRepository.GetUser(user.UserId, CancellationToken.None).Returns(user);

        _identity.UserId.Returns(user.UserId);

        _passwordManager.Validate(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        _passwordManager.Encrypt(Arg.Any<string>()).Returns(faker.Internet.Password());

        _userRepository.UpdateUser(user).Returns(Task.CompletedTask);

        var result = await _commandHandler.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}