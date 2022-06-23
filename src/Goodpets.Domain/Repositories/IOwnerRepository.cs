namespace Goodpets.Domain.Repositories;

public interface IOwnerRepository : IRepository
{
    Task Add(Owner owner, CancellationToken cancellationToken);
    Task<Owner?> Get(UserId userId, CancellationToken cancellationToken);
}