namespace Goodpets.Infrastructure.Database.EntityConfigurations;

internal class AccountConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(p => p.Email)
            .HasConversion(p => p.Value, p => Email.Create(p).Value).HasColumnOrder(4)
            .IsRequired().HasColumnName("Email");

        builder.Property(x => x.Password)
            .HasConversion(p => p.Value, p => Password.Create(p).Value).HasColumnOrder(1)
            .IsRequired().HasColumnName("Password");


        builder.Property(x => x.Username).HasConversion(p => p.Value, p => Username.Create(p).Value).HasColumnOrder(2)
            .IsRequired().HasMaxLength(12)
            .HasColumnName("UserName");

        builder.Property(x => x.Role).HasConversion(p => p.Value, p => Role.Create(p).Value).HasColumnOrder(3)
            .IsRequired().HasMaxLength(25)
            .HasColumnName("Role");

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