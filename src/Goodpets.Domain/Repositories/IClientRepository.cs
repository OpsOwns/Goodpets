namespace Goodpets.Domain.Repositories;

public interface IClientRepository : IRepository
{
    Task RegisterClient(Client client, CancellationToken cancellationToken);
}