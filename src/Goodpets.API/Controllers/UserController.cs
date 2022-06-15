namespace Goodpets.API.Controllers;

[Route($"{BasePath}/[controller]")]
internal class UserController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public UserController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [SwaggerOperation("Create the user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var command = new SignUp(userRegisterRequest.Email, userRegisterRequest.Password,
            userRegisterRequest.UserName);

        var result = await _dispatcher.SendAsync(command);

        if (result.IsFailed)
            return Ok(result.ToResultDto());


        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [SwaggerOperation("SignIn user and return the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccessTokenDto>> SignIn([FromBody] UserLoginRequest userLoginRequest)
    {
        var accessTokenDto =
            await _dispatcher.SendAsync(new SignIn(userLoginRequest.Login, userLoginRequest.Password));

        if (accessTokenDto.IsFailed)
            return Unauthorized(accessTokenDto.ToResult().ToResultDto());

        return Ok(accessTokenDto.Value);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [SwaggerOperation("Refresh the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _dispatcher.SendAsync(
            new Application.User.Commands.RefreshToken(refreshTokenRequest.AccessToken,
                refreshTokenRequest.RefreshToken));

        if (result.IsFailed)
            return BadRequest(result.ToResult().ToResultDto());

        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("sign-out")]
    [SwaggerOperation("SignOut user, revoke refresh token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutUser()
    {
        await _dispatcher.SendAsync(new SignOut());

        return NoContent();
    }
}