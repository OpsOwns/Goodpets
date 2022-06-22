namespace Goodpets.Infrastructure.Domain.Repositories;

internal class PetRepository : IPetRepository
{
    private readonly DbSet<Pet> _pets;

    public PetRepository(GoodpetsContext goodpetsContext)
    {
        _pets = goodpetsContext.Pets;
    }

    public async Task Add(Pet pet, CancellationToken cancellationToken)
    {
        await _pets.AddAsync(pet, cancellationToken);
    }
}