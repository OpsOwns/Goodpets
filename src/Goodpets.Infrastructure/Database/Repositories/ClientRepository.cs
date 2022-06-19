using Goodpets.Domain.Entities;
using Goodpets.Domain.Repositories;

namespace Goodpets.Infrastructure.Database.Repositories;

public class ClientRepository : IClientRepository
{
    public Task RegisterClient(Client client, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}