using Result = FluentResults.Result;

namespace Goodpets.Infrastructure.Dispatchers;

internal sealed class Dispatcher : IDispatcher
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public Dispatcher(ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public Task<Result> SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        return _commandDispatcher.SendAsync(command, cancellationToken);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return _queryDispatcher.QueryAsync(query, cancellationToken);
    }
}