namespace Goodpets.Infrastructure.LoggingCommand;

internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        var commandName = typeof(TCommand).Name.Underscore();
        _logger.LogInformation("Started handling a command: {CommandName}...", commandName);
        var result = await _commandHandler.HandleAsync(command, cancellationToken);
        _logger.LogInformation("Completed handling a command: {CommandName}.", commandName);

        return result;
    }
}