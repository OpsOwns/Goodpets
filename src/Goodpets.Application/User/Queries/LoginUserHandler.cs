using Goodpets.Infrastructure.Security.Abstractions;
using Goodpets.Infrastructure.Security.Auth;

namespace Goodpets.Application.User.Queries;

public record LoginUser(string Login, string Password) : IQuery<JwtToken>;

public class LoginUserHandler : IQueryHandler<LoginUser, JwtToken>
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserService _userService;

    public LoginUserHandler(ITokenHandler tokenHandler, IUserService userService)
    {
        _tokenHandler = tokenHandler;
        _userService = userService;
    }

    public async Task<JwtToken> HandleAsync(LoginUser query, CancellationToken cancellationToken = default)
    {
        var user = await _userService.Login(query.Login, query.Password, cancellationToken);

        var token = _tokenHandler.Create(user);

        return token;
    }
}