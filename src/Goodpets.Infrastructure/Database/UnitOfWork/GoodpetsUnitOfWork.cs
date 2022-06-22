using Result = FluentResults.Result;

namespace Goodpets.Infrastructure.Database.UnitOfWork;

sealed class GoodpetsUnitOfWork : IUnitOfWork
{
    private readonly GoodpetsContext _dbContext;

    public GoodpetsUnitOfWork(GoodpetsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> ExecuteAsync(Func<Task<Result>> action, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await action();

            if (result.IsFailed)
                return result;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}