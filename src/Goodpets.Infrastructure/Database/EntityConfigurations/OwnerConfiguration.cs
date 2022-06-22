namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(x => x.CustomerId);

        builder.Property(x => x.ContactEmail).HasConversion(z => z.Value,
            x => Email.Create(x).Value).IsRequired();

        builder.Property(x => x.PhoneNumber).HasConversion(z => z.Value,
            x => PhoneNumber.Create(x).Value).IsRequired();

        builder.OwnsOne(x => x.FullName, z =>
        {
            z.Property(x => x.Name).IsRequired();
            z.Property(x => x.SureName).IsRequired();
        });

        builder.OwnsOne(x => x.Address, z =>
        {
            z.Property(x => x.City).IsRequired();
            z.Property(x => x.Street).IsRequired();
            z.Property(x => x.ZipCode).IsRequired();
        });

        builder.HasMany(p => p.Pets).WithOne(p => p.Owner)
            .OnDelete(DeleteBehavior.Cascade)
            .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.ToTable("Owners", Schema.Goodpets);
    }
}