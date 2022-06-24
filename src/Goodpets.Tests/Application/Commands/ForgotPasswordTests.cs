namespace Goodpets.Tests.Application.Commands;

public class ForgotPasswordTests
{
    private readonly ICommandHandler<ForgotPassword> _commandHandler;

    private readonly IEmailService _emailService;
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _repository;

    public ForgotPasswordTests()
    {
        _emailService = Substitute.For<IEmailService>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _repository = Substitute.For<IUserRepository>();

        _commandHandler = new ForgotPasswordHandler(_repository, _emailService, _passwordManager);
    }


    [Fact]
    public async Task ForgotPassword_Invalid_User()
    {
        var faker = new Faker();
        var forgotPassword = new ForgotPassword(faker.Internet.Email());

        _repository.GetUserByEmail(Arg.Any<Email>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(null));

        var result = await _commandHandler.HandleAsync(forgotPassword, CancellationToken.None);


        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo($"User with email {forgotPassword.Email} not exists");
    }

    [Fact]
    public async Task ForgotPassword_Password_Changed_To_New()
    {
        var faker = new Faker();
        var forgotPassword = new ForgotPassword(faker.Internet.Email());
        var user = FakeUser.CreateFakeUser();

        _repository.GetUserByEmail(Arg.Any<Email>(), CancellationToken.None)
            .Returns(Task.FromResult<User?>(user));

        _passwordManager.GeneratePassword(Arg.Any<int>()).Returns("TestPassword12!Works");

        _passwordManager.Encrypt(Arg.Any<string>()).Returns("TestPassword12!Works");

        _repository.UpdateUser(user).Returns(Task.CompletedTask);

        _emailService.Send(Arg.Any<EmailMessage>(), CancellationToken.None).Returns(Task.CompletedTask);

        var result = await _commandHandler.HandleAsync(forgotPassword, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }
}