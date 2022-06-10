using System.Security.Cryptography;

namespace Goodpets.Infrastructure.Security;

internal sealed class TokenService : ITokenService
{
    private readonly string _issuer;
    private readonly TimeSpan _expiry;
    private readonly string _audience;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();
    private readonly IClock _clock;
    private readonly TokenValidationParameters _tokenValidationParameters;
    public DateTime ExpireDateToken => DateTime.Now.Add(_expiry);

    public TokenService(AuthenticationOptions options, IClock clock,
        TokenValidationParameters tokenValidationParameters)
    {
        _clock = clock;
        _tokenValidationParameters = tokenValidationParameters;
        _issuer = options.Issuer;
        _audience = options.Audience;
        _expiry = options.Expire ?? TimeSpan.FromHours(1);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }

    public string GenerateJwtToken(UserAccount userAccount)
    {
        var now = _clock.Current();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userAccount.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userAccount.Credentials.Username),
            new(JwtRegisteredClaimNames.Email, userAccount.Email.Value),
            new(ClaimTypes.Role, userAccount.Role.Value)
        };
        var expires = now.Add(_expiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        return _jwtSecurityToken.WriteToken(jwt);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");


        return principal;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}