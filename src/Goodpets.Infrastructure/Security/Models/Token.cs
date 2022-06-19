namespace Goodpets.Infrastructure.Security.Models;

internal record AccessToken(string Value, JwtId JwtId);

internal record RefreshToken(string Value, DateTime ExpireTime);

internal class Token
{
    internal string RefreshToken { get; set; } = null!;
    internal DateTime ExpireDate { get; set; }
    internal DateTime CreationDate { get; set; }
    internal bool Used { get; set; }
    internal Guid JwtId { get; set; }
}