namespace Goodpets.Infrastructure.Security;

internal sealed class TokenProvider : ITokenProvider
{
    private readonly string _audience;
    private readonly IClock _clock;
    private readonly TimeSpan _expiry;
    private readonly TimeSpan _expiryRefreshToken;
    private readonly string _issuer;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private ClaimsPrincipal? _claimsPrincipal;

    public TokenProvider(AuthenticationOptions options, IClock clock,
        TokenValidationParameters tokenValidationParameters)
    {
        _clock = clock;
        _tokenValidationParameters = tokenValidationParameters;
        _issuer = options.Issuer;
        _audience = options.Audience;
        _expiry = options.Expire ?? TimeSpan.FromHours(1);
        _expiryRefreshToken = options.ExpireRefreshToken ?? TimeSpan.FromHours(3);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }

    public AccessToken GenerateJwtToken(User user)
    {
        var now = _clock.Current();
        var jwtId = new JwtId();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, jwtId.Value.ToString()),
            new(ClaimTypes.Role, user.Role)
        };
        var expires = now.Add(_expiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var token = _jwtSecurityToken.WriteToken(jwt);

        return new AccessToken(token, jwtId);
    }

    public void ValidatePrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        _claimsPrincipal =
            tokenHandler.ValidateToken(token, _tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        return new RefreshToken(token, _clock.Current().Add(_expiryRefreshToken));
    }

    public Guid GetUserIdFromJwtToken()
    {
        if (_claimsPrincipal is null)
            throw new InvalidOperationException();

        return Guid.Parse(_claimsPrincipal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }

    public bool StoredJwtIdSameAsFromPrinciple(Guid storedJwtId)
    {
        if (storedJwtId == Guid.Empty)
            throw new ArgumentNullException(nameof(storedJwtId));

        if (_claimsPrincipal is null)
            throw new InvalidOperationException();

        Guid jwtId = Guid.Parse(_claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

        return jwtId == storedJwtId;
    }
}