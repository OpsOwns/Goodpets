[assembly: InternalsVisibleTo("Goodpets.API")]

namespace Goodpets.Shared.Api;

[ApiController]
[Route(BasePath)]
internal abstract class BaseController : ControllerBase
{
    protected const string BasePath = "goodpets";
}