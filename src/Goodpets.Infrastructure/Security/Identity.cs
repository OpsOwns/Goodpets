using Microsoft.AspNetCore.Http;

namespace Goodpets.Infrastructure.Security;

public class Identity : IIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Identity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public UserAccountId UserAccountId => _httpContextAccessor.HttpContext!.User.Claims
        .Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

    public JwtId JwtId => _httpContextAccessor.HttpContext!.User.Claims
        .Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
}