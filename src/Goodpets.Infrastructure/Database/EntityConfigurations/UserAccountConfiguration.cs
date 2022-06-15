using Goodpets.Infrastructure.Security.Models;

namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Password).IsRequired().HasColumnName("Password");
        builder.Property(x => x.Username).IsRequired().HasMaxLength(12)
            .HasColumnName("UserName");

        builder.Property(x => x.Role).IsRequired().HasMaxLength(25).HasColumnName("Role");

        builder.Property(x => x.Email).IsRequired().HasColumnName("Email");

        builder.ToTable("Accounts", Schema.Goodpets);
    }
}