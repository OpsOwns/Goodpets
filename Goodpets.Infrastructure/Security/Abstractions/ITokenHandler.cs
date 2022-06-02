namespace Goodpets.Infrastructure.Security.Abstractions;

public interface ITokenHandler
{
    public JwtToken Create(UserAccount userAccount);
}