namespace Goodpets.Application.SeedWork.Cqrs;

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

    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return _commandDispatcher.SendAsync(command, cancellationToken);
    }

    public Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        return _commandDispatcher.SendAsync(command, cancellationToken);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return _queryDispatcher.QueryAsync(query, cancellationToken);
    }
}