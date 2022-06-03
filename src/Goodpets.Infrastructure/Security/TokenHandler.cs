namespace Goodpets.Infrastructure.Security;

internal sealed class TokenHandler : ITokenHandler
{
    private readonly string _issuer;
    private readonly TimeSpan _expiry;
    private readonly string _audience;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();
    private readonly IClock _clock;

    public TokenHandler(AuthenticationOptions options, IClock clock)
    {
        _clock = clock;
        _issuer = options.Issuer;
        _audience = options.Audience;
        _expiry = options.Expire ?? TimeSpan.FromHours(1);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }

    public JwtToken Create(UserAccount userAccount)
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
        var token = _jwtSecurityToken.WriteToken(jwt);

        return new JwtToken(token);
    }
}