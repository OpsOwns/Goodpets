namespace Goodpets.Application.Abstractions;

public interface IIdentity
{
    UserId UserAccountId { get; }
    JwtId JwtId { get; }
    void Set(JsonWebToken jonWebToken);
    JsonWebToken? Get();
}