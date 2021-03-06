namespace Goodpets.Infrastructure.Security.Auth;

internal sealed class TokenProvider : ITokenProvider
{
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly TimeSpan _expiryRefreshToken;
    private readonly string _issuer;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private ClaimsPrincipal? _claimsPrincipal;

    public TokenProvider(AuthenticationOptions options,
        TokenValidationParameters tokenValidationParameters)
    {
        _tokenValidationParameters = tokenValidationParameters;
        _issuer = options.Issuer;
        _audience = options.Audience;
        _expiry = options.Expire ?? TimeSpan.FromHours(1);
        _expiryRefreshToken = options.ExpireRefreshToken ?? TimeSpan.FromHours(3);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }

    public AccessTokenDto GenerateJwtToken(User user)
    {
        var now = SystemClock.Instance.GetCurrentInstant().InUtc().ToDateTimeUtc();
        var jwtId = new JwtId();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username.Value),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Jti, jwtId.Value.ToString()),
            new(ClaimTypes.Role, user.Role)
        };
        var expires = now.Add(_expiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires,
            _signingCredentials);
        var token = _jwtSecurityToken.WriteToken(jwt);

        return new AccessTokenDto(token, jwtId);
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

    public RefreshTokenDto GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        return new RefreshTokenDto(token,
            SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc().Add(_expiryRefreshToken).ToLocalDateTime());
    }

    public UserId GetUserIdFromJwtToken()
    {
        if (_claimsPrincipal is null)
            throw new InvalidOperationException();

        return new UserId(Guid.Parse(_claimsPrincipal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value));
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