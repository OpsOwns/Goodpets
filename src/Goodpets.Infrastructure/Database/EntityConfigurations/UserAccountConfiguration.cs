namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("Accounts");
        builder.OwnsOne(x => x.Credentials, navigationBuilder =>
        {
            navigationBuilder.Property(credential => credential.Username).IsRequired().HasMaxLength(12)
                .HasColumnName("UserName");
            navigationBuilder.Property(credential => credential.Password).IsRequired().HasColumnName("Password");
        });

        builder.OwnsOne(x => x.Token, navigationBuilder =>
        {
            navigationBuilder.Property(token => token.RefreshToken).HasColumnName("RefreshToken").IsRequired(false);
            navigationBuilder.Property(token => token.Invalidated).HasColumnName("Invalidated").IsRequired(false);
            navigationBuilder.Property(token => token.CreationDate).HasColumnName("CreationDate").IsRequired(false);
            navigationBuilder.Property(token => token.ExpireDate).HasColumnName("ExpireDate").IsRequired(false);
            navigationBuilder.Property(token => token.Used).HasColumnName("Used").IsRequired(false);
        });

        builder.OwnsOne(x => x.Role, navigationBuilder => navigationBuilder.Property(role => role.Value)
            .IsRequired().HasMaxLength(25).HasColumnName("Role"));

        builder.OwnsOne(x => x.Email,
            navigationBuilder =>
            {
                navigationBuilder.Property(email => email.Value).IsRequired().HasMaxLength(36).HasColumnName("Email");
            });
    }
}