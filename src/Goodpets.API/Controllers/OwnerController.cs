namespace Goodpets.API.Controllers;

[Authorize]
[Route($"{BasePath}/[controller]")]
internal class OwnerController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public OwnerController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    [HttpPost("register")]
    [SwaggerOperation("Register new owner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterOwnerRequest registerOwnerRequest)
    {
        var result = await _dispatcher.SendAsync(new RegisterOwner(registerOwnerRequest.Name,
            registerOwnerRequest.SureName,
            registerOwnerRequest.ContactEmail, registerOwnerRequest.ZipCode, registerOwnerRequest.City,
            registerOwnerRequest.Street, registerOwnerRequest.Phone));

        if (result.IsFailed)
            return BadRequest(result.MapError());

        return Ok();
    }

    [HttpGet("details")]
    [SwaggerOperation("Get owner details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(OwnerDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OwnerDetailsDto>> Details()
    {
        var result = await _dispatcher.QueryAsync(new GetOwnerDetails());

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}