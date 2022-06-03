namespace Goodpets.Infrastructure.Security.Options;

internal sealed class AuthenticationOptions
{
    private const string SectionName = "Authentication";

    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SigningKey { get; init; }
    public TimeSpan? Expire { get; init; }


    public AuthenticationOptions(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var section = configuration.GetSection(SectionName);

        if (section is null)
        {
            throw new InvalidOperationException("Can't find configuration section");
        }

        Issuer = section.GetValue<string>(nameof(Issuer));
        Audience = section.GetValue<string>(nameof(Audience));
        SigningKey = section.GetValue<string>(nameof(SigningKey));
        Expire = section.GetValue<TimeSpan?>(nameof(Expire));
    }
}