namespace Goodpets.Shared.Api;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult OkOrNotFound<T>(T model)
    {
        if (model is not null)
        {
            return Ok(model);
        }

        return NotFound();
    }
}