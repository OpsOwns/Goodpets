using FluentResults;
using Goodpets.Application.Dto;
using Goodpets.Shared.Api.Dto;

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
    [HttpPost("register")]
    [SwaggerOperation("Create the user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        var command = new RegisterUser(userRegisterRequest.Email, userRegisterRequest.Password,
            userRegisterRequest.UserName);

        var result = await _dispatcher.SendAsync(command);

        if (result.IsFailed)
            return Ok(result.ToResultDto());


        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation("Login user and return the JSON Web Token")]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccessTokenDto>> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var accessTokenDto =
            await _dispatcher.SendAsync(new LoginUser(userLoginRequest.Login, userLoginRequest.Password));

        if (accessTokenDto.IsSuccess)
            return Ok(accessTokenDto.Value);

        return BadRequest(accessTokenDto.ToResult().ToResultDto());
    }

    [Authorize]
    [Consumes(RequestContentType.Json)]
    [Produces(RequestContentType.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        return Ok();
    }


    [Authorize(Policy = "admin")]
    [HttpGet("{userAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(Guid userAccountId)
    {
        return Ok(await _dispatcher.QueryAsync(
            new GetUserAccountById(new UserAccountId(userAccountId))));
    }
}