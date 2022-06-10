namespace Goodpets.Shared.Cqrs;
public static class Extensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddQueries();
        services.AddSingleton<IDispatcher, Dispatcher>();

        return services;
    }
}