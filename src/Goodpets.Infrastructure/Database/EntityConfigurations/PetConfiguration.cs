using NodaTime;

namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(x => x.PetId);

        builder.Property(x => x.DateOfBirth)
            .HasConversion(x => x.ToDateTimeUnspecified(), z => LocalDate.FromDateTime(z)).IsRequired();
        builder.Property(x => x.Breed).IsRequired();
        builder.Property(x => x.Coat).IsRequired();
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Weight).IsRequired();
        
        builder.HasOne(x => x.Owner).WithMany(x => x.Pets);
        
        
        builder.ToTable("Pets", Schema.Goodpets);
    }
}