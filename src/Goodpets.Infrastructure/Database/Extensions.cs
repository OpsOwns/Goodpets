using Goodpets.Infrastructure.Database.UnitOfWork;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Goodpets.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton(x => new DatabaseOptions(x.GetRequiredService<IConfiguration>()));
        services.AddTransient(x => new GoodpetsContext(x.GetRequiredService<DatabaseOptions>()));

        services.AddScoped<IUnitOfWork, GoodpetsUnitOfWork>();

        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkDecorator<>));

        services.AddTransient<ITokenRepository, TokenRepository>()
            .AddTransient<IUserRepository, UserRepository>();

        services.AddHostedService<DbInitializer>();


        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IRepository)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}