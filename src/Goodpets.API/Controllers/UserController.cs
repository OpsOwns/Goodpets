using Goodpets.Application.Commands.Auth;

namespace Goodpets.API.Controllers;

[Route($"{BasePath}/[controller]")]
internal class UserController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IIdentity _identity;

    public UserController(IDispatcher dispatcher, IIdentity identity)
    {
        _dispatcher = dispatcher;
        _identity = identity;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [SwaggerOperation("Create the user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] SignUpRequest signUpRequest)
    {
        var result = await _dispatcher.SendAsync(new SignUp(signUpRequest.UserName, signUpRequest.Password,
            signUpRequest.Email, signUpRequest.Role), HttpContext.RequestAborted);

        if (result.IsFailed)
            return BadRequest(result.MapError());


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
    public async Task<ActionResult<JsonWebToken>> SignIn([FromBody] SignInRequest signInRequest)
    {
        var result = await _dispatcher.SendAsync(new SignIn(signInRequest.UserName, signInRequest.Password),
            HttpContext.RequestAborted);

        if (result.IsFailed)
            return Unauthorized(result.MapError());

        return Ok(_identity.Get());
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
        var result = await _dispatcher.SendAsync(
            new RefreshToken(jsonWebTokenRequest.AccessToken, jsonWebTokenRequest.RefreshToken),
            HttpContext.RequestAborted);

        if (result.IsFailed)
            return BadRequest(result.MapError());

        return Ok(_identity.Get());
    }


    [Authorize]
    [HttpPut("change-password")]
    [SwaggerOperation("Change user password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var result = await _dispatcher.SendAsync(
            new ChangePassword(changePasswordRequest.OldPassword, changePasswordRequest.NewPassword),
            HttpContext.RequestAborted);

        if (result.IsFailed)
            return BadRequest(result.MapError());

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [SwaggerOperation("Send new password to email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
    {
        var result =
            await _dispatcher.SendAsync(new ForgotPassword(forgotPasswordRequest.Email), HttpContext.RequestAborted);

        if (result.IsFailed)
            return NotFound(result.MapError());

        return Ok();
    }

    [Authorize]
    [HttpDelete("sign-out")]
    [SwaggerOperation("SignOut user, revoke refresh token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutUser()
    {
        await _dispatcher.SendAsync(new SignOut(), HttpContext.RequestAborted);

        return NoContent();
    }
}