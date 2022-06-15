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
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var result = await _userService.SignUp(userRegisterRequest.UserName, userRegisterRequest.Password,
            userRegisterRequest.Email, userRegisterRequest.Role, CancellationToken.None);

        if (result.IsFailed)
            return BadRequest(result.ToResultDto());


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
            await _userService.SignIn(userLoginRequest.Login, userLoginRequest.Password, CancellationToken.None);

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
        var result = await _userService.RefreshToken(refreshTokenRequest.AccessToken,
            refreshTokenRequest.RefreshToken, CancellationToken.None);

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
        await _userService.SignOut(CancellationToken.None);

        return NoContent();
    }
}