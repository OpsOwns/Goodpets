namespace Goodpets.Tests.Application.Commands;

public class SignOutTests
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentity _identity;
    private readonly ICommandHandler<SignOut> _commandHandler;

    public SignOutTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _identity = Substitute.For<IIdentity>();
        _commandHandler = new SignOutHandler(_userRepository, _identity);
    }

    [Fact]
    public async Task SingOut_Invalid_Not_Found_User()
    {
        var userId = new UserId();
        var signOut = new SignOut();

        _identity.UserId.Returns(userId);

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(null));

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(signOut, CancellationToken.None));

        exception.ShouldBeOfType<BusinessException>();
        exception.Message.ShouldBeEquivalentTo("System unable to find user");
    }

    [Fact]
    public async Task SingOut_Success()
    {
        var userId = new UserId();
        var signOut = new SignOut();

        _identity.UserId.Returns(userId);

        _userRepository.GetUser(Arg.Any<UserId>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(FakeUser.CreateFakeUser()));

        _userRepository.UpdateUser(Arg.Any<User>()).Returns(Task.CompletedTask);

        var result = await _commandHandler.HandleAsync(signOut, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}