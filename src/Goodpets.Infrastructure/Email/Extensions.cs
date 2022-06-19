namespace Goodpets.Infrastructure.Email;

public static class Extensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services.TryAddTransient<IEmailService, EmailService>();
        services.AddSingleton(x => new EmailOptions(x.GetRequiredService<IConfiguration>()));


        return services;
    }
}