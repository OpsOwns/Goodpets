namespace Goodpets.Infrastructure.Database.Queries.Handlers;

internal class GetOwnerDetailsHandler : IQueryHandler<GetOwnerDetails, OwnerDetailsDto?>
{
    private readonly IIdentity _identity;
    private readonly DbSet<Owner> _owners;

    public GetOwnerDetailsHandler(GoodpetsContext goodpetsContext, IIdentity identity)
    {
        _identity = identity;
        _owners = goodpetsContext.Owners;
    }

    public async Task<OwnerDetailsDto?> HandleAsync(GetOwnerDetails query,
        CancellationToken cancellationToken = default)
    {
        var owner = await _owners.SingleOrDefaultAsync(x => x.UserId == _identity.UserId, cancellationToken);

        if (owner is null)
            return null;

        return new OwnerDetailsDto(owner.FullName.Name, owner.FullName.SureName, owner.ContactEmail.Value);
    }
}