namespace Goodpets.Infrastructure.Emails;

public static class Extensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.TryAddTransient<IEmailService, EmailService>();
        services.AddSingleton(x => new EmailOptions(x.GetRequiredService<IConfiguration>()));


        return services;
    }
}