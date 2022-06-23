namespace Goodpets.Application.Abstractions.SeedWork;

public interface ICommandDispatcher
{
    
    Task<Result> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand;
}