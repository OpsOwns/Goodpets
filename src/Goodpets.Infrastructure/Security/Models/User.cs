namespace Goodpets.Infrastructure.Security.Models;

internal class User
{
    internal Guid Id { get; set; }
    internal string Role { get; set; } = null!;
    internal string Username { get; set; } = null!;
    internal string Password { get; set; } = null!;
    internal string Email { get; set; } = null!;
}