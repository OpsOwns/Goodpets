namespace Goodpets.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSecurity();
        services.AddSingleton<IClock, Clock>();
        services.AddDatabase();

        return services;
    }
}