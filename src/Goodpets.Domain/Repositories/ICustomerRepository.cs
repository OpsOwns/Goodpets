namespace Goodpets.Domain.Repositories;

public interface ICustomerRepository : IRepository
{
    Task Register(Owner customer, CancellationToken cancellationToken);
}