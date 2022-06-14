using Goodpets.Application.SeedWork.Cqrs;

namespace Goodpets.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IService)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddCqrs();

        return services;
    }
}