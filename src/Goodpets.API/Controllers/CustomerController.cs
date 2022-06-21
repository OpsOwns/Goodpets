namespace Goodpets.API.Controllers;

[Authorize]
[Route($"{BasePath}/[controller]")]
internal class CustomerController : BaseController
{
}