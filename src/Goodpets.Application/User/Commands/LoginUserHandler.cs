namespace Goodpets.Application.User.Commands;

public record LoginUser(string Login, string Password) : ICommand<Result>;

public class LoginUserHandler : ICommandHandler<LoginUser, Result>
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public LoginUserHandler(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }


    public async Task<Result> HandleAsync(LoginUser query, CancellationToken cancellationToken = default)
    {
        var result = await _userService.Login(query.Login, query.Password, cancellationToken);
        
        if (result.IsFailed)
            return result.ToResult();

        return Result.Ok();
    }
}