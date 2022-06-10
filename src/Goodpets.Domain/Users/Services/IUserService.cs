using Goodpets.Shared.Abstractions;

namespace Goodpets.Domain.Users.Services;

public interface IUserService : IService
{
    Task<Result<UserAccount>> RegisterUser(string username, string password, string email,
        CancellationToken cancellationToken);

    Task<Result<UserAccount>> Login(string username, string password, CancellationToken cancellationToken);
}