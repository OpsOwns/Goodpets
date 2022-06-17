namespace Goodpets.Infrastructure.Database;

public class DbInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(IServiceProvider serviceProvider, ILogger<DbInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        await scope.ServiceProvider.GetRequiredService<GoodpetsContext>().Database
            .EnsureCreatedAsync(cancellationToken);

        _logger.LogInformation("Migrate database success");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}