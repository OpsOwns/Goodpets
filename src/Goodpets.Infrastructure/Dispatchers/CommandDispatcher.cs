namespace Goodpets.Infrastructure.Dispatchers;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
}