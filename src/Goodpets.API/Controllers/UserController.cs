namespace Goodpets.API.Controllers;

[Route($"{BasePath}/[controller]")]
internal class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [SwaggerOperation("Create the user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var result = await _userService.SignUp(userRegisterRequest.UserName, userRegisterRequest.Password,
            userRegisterRequest.Email, userRegisterRequest.Role, HttpContext.RequestAborted);

        if (result.IsFailed)
            return BadRequest(result.MapToError());


        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [SwaggerOperation("SignIn user and return the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(JsonWebToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> SignIn([FromBody] UserLoginRequest userLoginRequest)
    {
        var jsonWebToken =
            await _userService.SignIn(userLoginRequest.Login, userLoginRequest.Password, HttpContext.RequestAborted);

        if (jsonWebToken.IsFailed)
            return Unauthorized(jsonWebToken.ToResult().MapToError());

        return Ok(jsonWebToken.Value);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [SwaggerOperation("Refresh the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(JsonWebToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> RefreshToken([FromBody] JsonWebTokenRequest jsonWebTokenRequest)
    {
        var jsonWebToken = await _userService.RefreshToken(jsonWebTokenRequest.AccessToken,
            jsonWebTokenRequest.RefreshToken, HttpContext.RequestAborted);

        if (jsonWebToken.IsFailed)
            return BadRequest(jsonWebToken.ToResult().MapToError());

        return Ok(jsonWebToken.Value);
    }

    [Authorize]
    [HttpDelete("sign-out")]
    [SwaggerOperation("SignOut user, revoke refresh token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutUser()
    {
        await _userService.SignOut(HttpContext.RequestAborted);

        return NoContent();
    }
}