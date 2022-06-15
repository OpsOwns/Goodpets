namespace Goodpets.Infrastructure.Security.Options;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public ConfigureJwtBearerOptions(AuthenticationOptions authenticationOptions,
        TokenValidationParameters tokenValidationParameters)
    {
        _authenticationOptions =
            authenticationOptions ?? throw new ArgumentNullException(nameof(authenticationOptions));
        _tokenValidationParameters = tokenValidationParameters ??
                                     throw new ArgumentNullException(nameof(tokenValidationParameters));
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
            return;

        options.MapInboundClaims = false;
        options.Audience = _authenticationOptions.Audience;
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = _tokenValidationParameters;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }
}