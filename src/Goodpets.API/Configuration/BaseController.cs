namespace Goodpets.API.Configuration;

[ApiController]
[Route(BasePath)]
internal abstract class BaseController : ControllerBase
{
    protected const string BasePath = "goodpets";
}