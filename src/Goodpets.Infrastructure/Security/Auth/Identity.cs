namespace Goodpets.Infrastructure.Security.Auth;

public class Identity : IIdentity
{
    private const string TokenKey = "jwt";
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

    public void Set(JsonWebToken jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JsonWebToken? Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JsonWebToken;
        }

        return null;
    }

    public UserId UserId

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