namespace Goodpets.Infrastructure.Database.UnitOfWork;

internal class GoodpetsUnitOfWork : IUnitOfWork
{
    private readonly GoodpetsContext _dbContext;

    public GoodpetsUnitOfWork(GoodpetsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await action();
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}