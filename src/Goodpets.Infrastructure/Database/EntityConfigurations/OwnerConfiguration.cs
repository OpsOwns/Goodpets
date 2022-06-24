namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(x => x.CustomerId);

        builder.Property(x => x.ContactEmail).HasConversion(z => z.Value,
            x => Email.Create(x).Value).IsRequired().HasColumnName("ContactEmail");

        builder.Property(x => x.PhoneNumber).HasConversion(z => z.Value,
            x => PhoneNumber.Create(x).Value).IsRequired().HasColumnName("PhoneNumber");

        builder.OwnsOne(x => x.FullName, z =>
        {
            z.Property(x => x.Name).IsRequired().HasColumnName("FirstName");
            z.Property(x => x.SureName).IsRequired().HasColumnName("SureName");
        });

        builder.OwnsOne(x => x.Address, z =>
        {
            z.Property(x => x.City).IsRequired().HasColumnName("City");
            z.Property(x => x.Street).IsRequired().HasColumnName("Street");
            z.Property(x => x.ZipCode).IsRequired().HasColumnName("ZipCode");
        });

        builder.HasMany(p => p.Pets).WithOne(p => p.Owner)
            .OnDelete(DeleteBehavior.Cascade)
            .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("Owners", Schema.Goodpets);
    }
}