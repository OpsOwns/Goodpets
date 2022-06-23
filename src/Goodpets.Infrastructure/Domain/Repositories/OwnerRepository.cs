namespace Goodpets.Infrastructure.Domain.Repositories;

internal class OwnerRepository : IOwnerRepository
{
    private readonly DbSet<Owner> _owners;

    public OwnerRepository(GoodpetsContext goodpetsContext)
    {
        _owners = goodpetsContext.Owners;
    }

    public async Task Add(Owner owner, CancellationToken cancellationToken)
    {
        if (owner is null)
            throw new OwnerException(nameof(Owner), "can't be null");

        await _owners.AddAsync(owner, cancellationToken);
    }

    public async Task<Owner?> Get(UserId userId, CancellationToken cancellationToken)
    {
        return await _owners.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? null;
    }
}