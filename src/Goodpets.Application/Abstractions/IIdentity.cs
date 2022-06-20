namespace Goodpets.Application.Abstractions;

public interface IIdentity
{
    UserId UserId { get; }
    JwtId JwtId { get; }
    void Set(JsonWebToken jonWebToken);
    JsonWebToken? Get();
}