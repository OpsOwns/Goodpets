namespace Goodpets.Application.User.Commands;

public record SignUp(string Email, string Password, string Username) : ICommand<Result>;

public class SignUprHandler : ICommandHandler<SignUp, Result>
{
    private readonly IUserService _registerUserService;
    private readonly IUserAccountRepository _repository;

    public SignUprHandler(IUserAccountRepository repository, IUserService registerUserService)
    {
        _repository = repository;
        _registerUserService = registerUserService;
    }

    public async Task<Result> HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var user = await _registerUserService.RegisterUser(command.Username, command.Password, command.Email,
            cancellationToken);

        if (user.IsFailed) return user.ToResult();

        await _repository.Add(user.Value, cancellationToken);

        return Result.Ok();
    }
}