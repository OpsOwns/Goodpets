namespace Goodpets.Infrastructure.Security.Options;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _authenticationOptions;

    public ConfigureJwtBearerOptions(AuthenticationOptions authenticationOptions)
    {
        _authenticationOptions =
            authenticationOptions ?? throw new ArgumentNullException(nameof(authenticationOptions));
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
            return;

        options.Audience = _authenticationOptions.Audience;
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _authenticationOptions.Issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.SigningKey))
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }
}