namespace Goodpets.Infrastructure.Database.EntityConfigurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnOrder(0);

        builder.Property(x => x.RefreshToken).HasColumnName("RefreshToken").IsRequired();
        builder.Property(x => x.Used).HasColumnName("Used").IsRequired();
        builder.Property(x => x.ExpireDate).HasColumnName("ExpireDate").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CreationDate").IsRequired();

        builder.Property(x => x.UserAccountId).HasColumnOrder(1).HasConversion(x => x.Value,
            c => new UserAccountId(c)).HasColumnName("UserAccountId").IsRequired();

        builder.Property(x => x.JwtId).HasColumnOrder(2).HasConversion(x => x.Value,
            c => new JwtId(c)).HasColumnName("JwtId").IsRequired();

        builder.ToTable("RefreshTokens", Schema.Goodpets);
    }
}