namespace Goodpets.Infrastructure.Security.Options;

internal sealed class AuthenticationOptions
{
    private const string SectionName = "Authentication";


    public AuthenticationOptions(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var section = configuration.GetSection(SectionName);

        if (section is null) throw new InvalidOperationException("Can't find configuration section");

        Issuer = section.GetValue<string>(nameof(Issuer));
        Audience = section.GetValue<string>(nameof(Audience));
        SigningKey = section.GetValue<string>(nameof(SigningKey));
        Expire = section.GetValue<TimeSpan?>(nameof(Expire));
        ExpireRefreshToken = section.GetValue<TimeSpan?>(nameof(ExpireRefreshToken));
    }

    internal string Issuer { get; init; }
    internal string Audience { get; init; }
    internal string SigningKey { get; init; }
    internal TimeSpan? Expire { get; init; }
    internal TimeSpan? ExpireRefreshToken { get; init; }
}