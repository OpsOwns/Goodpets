[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Goodpets.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton(x => new DatabaseOptions(x.GetRequiredService<IConfiguration>()));
        services.AddScoped(x => new GoodpetsContext(x.GetRequiredService<DatabaseOptions>()));

        services.AddScoped<IUnitOfWork, GoodpetsUnitOfWork>();

        services.AddHostedService<DbInitializer>();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkDecorator<>));

        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IRepository)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}