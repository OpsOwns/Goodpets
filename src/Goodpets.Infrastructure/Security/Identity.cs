namespace Goodpets.Infrastructure.Security;

public class Identity : IIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Identity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public JwtId JwtId
    {
        get
        {
            if (_httpContextAccessor.HttpContext?.User is null)
            {
                throw new InvalidOperationException("HttpContext is null or HttpContext.User");
            }

            var jwtClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);


            if (jwtClaim is null)
            {
                throw new InvalidOperationException($"Claim {JwtRegisteredClaimNames.Jti} not found");
            }

            return jwtClaim.Value;
        }
    }

    public UserAccountId UserAccountId

    {
        get
        {
            if (_httpContextAccessor.HttpContext?.User is null)
            {
                throw new InvalidOperationException("HttpContext is null or HttpContext.User");
            }

            var userAccountClaim =
                _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userAccountClaim is null)
            {
                throw new InvalidOperationException($"Claim {ClaimTypes.NameIdentifier} not found");
            }

            return userAccountClaim.Value;
        }
    }
}