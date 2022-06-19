namespace Goodpets.Application.Abstractions;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}