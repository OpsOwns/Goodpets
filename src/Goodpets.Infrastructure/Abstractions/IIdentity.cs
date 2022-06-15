namespace Goodpets.Infrastructure.Abstractions;

public interface IIdentity
{
    UserAccountId UserAccountId { get; }
    JwtId JwtId { get; }
}