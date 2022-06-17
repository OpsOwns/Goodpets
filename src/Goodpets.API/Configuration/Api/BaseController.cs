namespace Goodpets.API.Configuration.Api;

[ApiController]
[Route(BasePath)]
internal abstract class BaseController : ControllerBase
{
    protected const string BasePath = "goodpets";
}