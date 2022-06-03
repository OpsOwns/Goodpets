using Goodpets.Infrastructure.Database.EntityConfigurations;
using Goodpets.Infrastructure.Database.Options;

namespace Goodpets.Infrastructure.Database;

public class GoodpetsContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions = null!;

    internal DbSet<UserAccount> UserAccount => Set<UserAccount>();

    public GoodpetsContext()
    {
    }

    public GoodpetsContext(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_databaseOptions.ConnectionString);
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole()))
            .EnableSensitiveDataLogging();
        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
    }
}