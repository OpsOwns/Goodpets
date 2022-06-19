namespace Goodpets.Infrastructure.Database.EntityConfigurations;

internal class AccountConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnOrder(0);

        builder.Property(x => x.Password).HasColumnOrder(1).IsRequired().HasColumnName("Password");
        builder.Property(x => x.Username).HasColumnOrder(2).IsRequired().HasMaxLength(12)
            .HasColumnName("UserName");

        builder.Property(x => x.Role).HasColumnOrder(3).IsRequired().HasMaxLength(25).HasColumnName("Role");

        builder.Property(x => x.Email).HasColumnOrder(4).IsRequired().HasColumnName("Email");

        builder.OwnsOne(x => x.Token, z =>
        {
            z.Property(x => x.RefreshToken).HasColumnName("RefreshToken").IsRequired();
            z.Property(x => x.Used).HasColumnName("Used").IsRequired();
            z.Property(x => x.ExpireDate).HasColumnName("ExpireDate").IsRequired();
            z.Property(x => x.CreationDate).HasColumnName("CreationDate").IsRequired();

            z.Property(x => x.JwtId).HasColumnName("JwtId").IsRequired();
        });

        builder.ToTable("Users", Schema.Goodpets);
    }
}