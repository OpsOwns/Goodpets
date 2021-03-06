namespace Goodpets.Infrastructure.Database;

internal class GoodpetsContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    public GoodpetsContext(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    internal DbSet<User> Users => Set<User>();
    internal DbSet<Pet> Pets => Set<Pet>();
    internal DbSet<Owner> Owners => Set<Owner>();

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