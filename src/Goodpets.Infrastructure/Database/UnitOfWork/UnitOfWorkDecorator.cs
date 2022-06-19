namespace Goodpets.Infrastructure.Database.UnitOfWork;

public class UnitOfWorkDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkDecorator(ICommandHandler<TCommand> commandHandler, IUnitOfWork unitOfWork)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteAsync(() => _commandHandler.HandleAsync(command, cancellationToken));
        return Result.Ok();
    }
}