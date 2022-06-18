namespace Goodpets.Infrastructure.Email;

public static class Extensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton(x => new EmailOptions(x.GetRequiredService<IConfiguration>()));


        return services;
    }
}