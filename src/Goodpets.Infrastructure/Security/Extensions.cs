using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Goodpets.Infrastructure.Security;

public static class Extensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddSingleton(x =>
            {
                var authenticationOptions = x.GetRequiredService<AuthenticationOptions>();
                return new TokenValidationParameters
                {
                    ValidIssuer = authenticationOptions.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    RequireAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.SigningKey)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            })
            .AddSingleton<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>()
            .AddSingleton<IPasswordEncryptor, PasswordEncryptor>()
            .AddSingleton<ITokenProvider, TokenProvider>()
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null!);

        services.TryAddSingleton<IIdentity, Identity>();

        services.AddAuthorization(authorization =>
        {
            authorization.AddPolicy("admin", policy => { policy.RequireRole("admin"); });
            authorization.AddPolicy("user", policy => { policy.RequireRole("user"); });
        });

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddSingleton(x => new AuthenticationOptions(x.GetRequiredService<IConfiguration>()));

        return services;
    }
}