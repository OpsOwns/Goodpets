namespace Goodpets.Application.Abstractions.Security;

public interface IIdentity
{
    UserAccountId UserAccountId { get; }
    JwtId JwtId { get; }
}