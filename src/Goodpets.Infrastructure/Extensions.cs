using Goodpets.Application.Abstractions.Time;

namespace Goodpets.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddSecurity();
        services.AddSingleton<IClock, Clock>();
        services.AddDatabase();

        return services;
    }
}