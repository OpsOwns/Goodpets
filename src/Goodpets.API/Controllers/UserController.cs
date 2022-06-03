namespace Goodpets.API.Controllers;

[Route($"{BasePath}/[controller]")]
public class UserController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public UserController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost("register")]
    [SwaggerOperation("Create the user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var command = new RegisterUser(userRegisterRequest.Email, userRegisterRequest.Password,
            userRegisterRequest.UserName);

        await _dispatcher.SendAsync(command);

        return Ok();
    }

    [HttpPost("login")]
    [SwaggerOperation("Login user and return the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var jwtToken =
            await _dispatcher.QueryAsync(new LoginUser(userLoginRequest.Login, userLoginRequest.Password));

        return OkOrNotFound(jwtToken);
    }

    [Authorize(Policy = "admin")]
    [HttpGet("{userAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(Guid userAccountId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(
            new GetUserAccountById(new UserAccountId(userAccountId))));
    }
}