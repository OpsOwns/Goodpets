namespace Goodpets.Infrastructure.Security;

public static class Extensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services
            .AddSingleton<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>()
            .AddSingleton<IPasswordEncryptor, PasswordEncryptor>()
            .AddSingleton<ITokenHandler, TokenHandler>()
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null!);

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