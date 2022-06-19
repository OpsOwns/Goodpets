using Goodpets.Application.Abstractions;
using Goodpets.Application.Abstractions.Email;
using Goodpets.Application.Commands.Auth;

namespace Goodpets.Application.Commands.Handlers;

public class AuthHandler : ICommandHandler<SignUp>, ICommandHandler<SignIn>, ICommandHandler<RefreshToken>,
    ICommandHandler<ChangePassword>, ICommandHandler<ForgotPassword>, ICommandHandler<SignOut>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IEmailService _emailService;

    public AuthHandler(IIdentityProvider identityProvider, IEmailService emailService)
    {
        _identityProvider = identityProvider;
        _emailService = emailService;
    }

    public async Task<Result> HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return await _identityProvider.SignUp(command.Username, command.Password, command.Email, command.Role,
            cancellationToken);
    }

    public async Task<Result> HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return await _identityProvider.SignIn(command.UserName, command.Password, cancellationToken);
    }

    public async Task<Result> HandleAsync(RefreshToken command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return await _identityProvider.RefreshToken(command.AccessToken, command.Token, cancellationToken);
    }

    public async Task<Result> HandleAsync(ChangePassword command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        return await _identityProvider.ChangePassword(command.NewPassword, command.OldPassword, cancellationToken);
    }

    public async Task<Result> HandleAsync(ForgotPassword command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var password = await _identityProvider.ResetPassword(command.Email, cancellationToken);

        if (password.IsFailed)
            return password.ToResult();

        await _emailService.Send(new EmailMessage(password.Value, command.Email, "[GoodPets] New password"),
            cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> HandleAsync(SignOut command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        await _identityProvider.SignOut(cancellationToken);

        return Result.Ok();
    }
}