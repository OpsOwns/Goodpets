namespace Goodpets.Infrastructure.Dispatchers;

public static class Extensions
{
    public static IServiceCollection AddDispatchers(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICommandHandler<>).Assembly;

        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddSingleton<IDispatcher, Dispatcher>();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        return services;
    }
}