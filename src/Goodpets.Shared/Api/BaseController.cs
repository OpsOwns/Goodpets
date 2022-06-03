namespace Goodpets.Shared.Api;

[ApiController]
[Route(BasePath)]
public abstract class BaseController : ControllerBase
{
    protected const string BasePath = "goodpets";

    protected IActionResult OkOrNotFound<T>(T model)
    {
        if (model is not null)
        {
            return Ok(model);
        }

        return NotFound();
    }
}