namespace Goodpets.Infrastructure.Database.EntityConfigurations;

internal class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnOrder(0);

        builder.Property(x => x.RefreshToken).HasColumnName("RefreshToken").IsRequired();
        builder.Property(x => x.Used).HasColumnName("Used").IsRequired();
        builder.Property(x => x.ExpireDate).HasColumnName("ExpireDate").IsRequired();
        builder.Property(x => x.CreationDate).HasColumnName("CreationDate").IsRequired();

        builder.HasOne(x => x.User).WithOne().HasForeignKey<Token>(x => x.UserId);

        builder.Property(x => x.JwtId).HasColumnOrder(2).HasColumnName("JwtId").IsRequired();

        builder.ToTable("RefreshTokens", Schema.Goodpets);
    }
}