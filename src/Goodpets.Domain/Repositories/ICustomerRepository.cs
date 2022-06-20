using Goodpets.Domain.Abstractions;

namespace Goodpets.Domain.Repositories;

public interface ICustomerRepository : IRepository
{
    Task Register(Customer customer, CancellationToken cancellationToken);
}