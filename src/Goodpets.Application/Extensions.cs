using Goodpets.Application.Cqrs;

namespace Goodpets.Application;

public static class Extensions
{
    public static Error WithErrorCode(this Error error, string propertyName)
    {
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));

        error.WithMetadata("ErrorParameter", propertyName);
        return error;
    }

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