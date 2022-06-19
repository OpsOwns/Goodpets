namespace Goodpets.Infrastructure.Abstractions;

public interface IUnitOfWork
{
    Task<Result> ExecuteAsync(Func<Task<Result>> action, CancellationToken cancellationToken);
}