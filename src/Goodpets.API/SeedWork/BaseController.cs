namespace Goodpets.API.SeedWork;

[ApiController]
[Route(BasePath)]
internal abstract class BaseController : ControllerBase
{
    protected const string BasePath = "goodpets";
}