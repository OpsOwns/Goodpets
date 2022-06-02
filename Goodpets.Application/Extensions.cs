namespace Goodpets.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddRepositories();
        service.AddDomainServices();
        service.AddCqrs();
        
        return service;
    }
}