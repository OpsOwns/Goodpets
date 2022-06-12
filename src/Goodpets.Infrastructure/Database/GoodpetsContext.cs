﻿namespace Goodpets.Infrastructure.Database;

public class GoodpetsContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    internal DbSet<UserAccount> UserAccount => Set<UserAccount>();
    internal DbSet<Token> Tokens => Set<Token>();

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