using Goodpets.Application.SeedWork;

namespace Goodpets.Application.User.Commands.Handlers;

internal sealed class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordManager _passwordManager;

    public ForgotPasswordHandler(IUserRepository userRepository, IEmailService emailService,
        IPasswordManager passwordManager)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _passwordManager = passwordManager;
    }

    public async Task<Result> HandleAsync(ForgotPassword command, CancellationToken cancellationToken = default)
    {
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
}