﻿namespace Goodpets.Application.SeedWork.Cqrs.Commands;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync));
        if (method is null)
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");

        return await (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken })!;
    }
}