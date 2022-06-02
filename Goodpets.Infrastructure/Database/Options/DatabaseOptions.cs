namespace Goodpets.Infrastructure.Database.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; init; }

    public DatabaseOptions(IConfiguration configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        ConnectionString = configuration.GetValue<string>(nameof(ConnectionString));

        if (string.IsNullOrEmpty(ConnectionString))
            throw new ArgumentException($"{nameof(ConnectionString)} can't be null or empty");
    }
}