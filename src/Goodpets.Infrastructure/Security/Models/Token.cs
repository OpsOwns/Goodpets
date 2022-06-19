namespace Goodpets.Infrastructure.Security.Models;

internal record AccessToken(string Value, JwtId JwtId);

internal record RefreshToken(string Value, DateTime ExpireTime);

internal class Token
{
    internal Guid Id { get; set; }
    internal string RefreshToken { get; set; } = null!;
    internal DateTime ExpireDate { get; set; }
    internal DateTime CreationDate { get; set; }
    internal bool Used { get; set; }
    internal Guid JwtId { get; set; }
    internal Guid UserId { get; set; }
    internal virtual User User { get; set; } = null!;
}