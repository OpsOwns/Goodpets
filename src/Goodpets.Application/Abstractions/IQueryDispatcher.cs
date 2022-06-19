namespace Goodpets.Application.Abstractions;

public interface IQueryDispatcher
{
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}