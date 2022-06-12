namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Credentials, navigationBuilder =>
        {
            navigationBuilder.Property(credential => credential.Username).IsRequired().HasMaxLength(12)
                .HasColumnName("UserName");
            navigationBuilder.Property(credential => credential.Password).IsRequired().HasColumnName("Password");
        });

        builder.OwnsOne(x => x.Role, navigationBuilder => navigationBuilder.Property(role => role.Value)
            .IsRequired().HasMaxLength(25).HasColumnName("Role"));

        builder.OwnsOne(x => x.Email,
            navigationBuilder =>
            {
                navigationBuilder.Property(email => email.Value).IsRequired().HasMaxLength(36).HasColumnName("Email");
            });

        builder.ToTable("Accounts", Schema.Goodpets);
    }
}