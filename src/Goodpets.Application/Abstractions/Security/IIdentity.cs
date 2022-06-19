using Goodpets.Application.Dto;

namespace Goodpets.Application.Abstractions.Security;

public interface IIdentity
{
    UserId UserAccountId { get; }
    JwtId JwtId { get; }
    void Set(JsonWebToken jonWebToken);
    JsonWebToken? Get();
}