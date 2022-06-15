namespace Goodpets.Infrastructure.Security.Models;

public class Token
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime CreationDate { get; set; }
    public bool Used { get; set; }
    public Guid JwtId { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}