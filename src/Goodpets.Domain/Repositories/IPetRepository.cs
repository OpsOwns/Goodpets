namespace Goodpets.Domain.Repositories;

public interface IPetRepository : IRepository
{
    Task Add(Pet pet, CancellationToken cancellationToken);
}