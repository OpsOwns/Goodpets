using Goodpets.Infrastructure.Database.Options;

namespace Goodpets.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton(x => new DatabaseOptions(x.GetRequiredService<IConfiguration>()));
        services.AddTransient(x => new GoodpetsContext(x.GetRequiredService<DatabaseOptions>()));
        
        return services;
    }
}