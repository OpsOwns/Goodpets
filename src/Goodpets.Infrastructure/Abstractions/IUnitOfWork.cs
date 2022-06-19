namespace Goodpets.Infrastructure.Abstractions;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}