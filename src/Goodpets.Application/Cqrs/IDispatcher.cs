namespace Goodpets.Application.Cqrs;

public interface IDispatcher
{
    Task<Result> SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand;
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}