namespace Goodpets.Application.Abstractions.Security;

public interface IIdentity
{
    UserId UserId { get; }
    JwtId JwtId { get; }
    void Set(JsonWebToken jonWebToken);
    JsonWebToken? Get();
}