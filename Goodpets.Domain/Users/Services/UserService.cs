using Goodpets.Domain.Abstractions;

namespace Goodpets.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserAccountRepository _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserService(IUserAccountRepository repository, IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
    }


    public async Task<UserAccount> RegisterUser(string username, string password, string email,
        CancellationToken cancellationToken)
    {
        var credentials = Credentials.Create(username, _passwordEncryptor.Encrypt(password));
        var userEmail = Email.Create(email);

        if (await _repository.CheckUserExistsByEmail(userEmail, cancellationToken))
        {
            throw new BusinessException($"User with email {userEmail.Value} already exists in system");
        }

        return new UserAccount(new UserAccountId(), Role.User(), credentials, userEmail);
    }

    public async Task<UserAccount> Login(string username, string password, CancellationToken cancellationToken)
    {
        var credentials = Credentials.Create(username, password);

        var user = await _repository.GetUserAccount(credentials.Username, cancellationToken);

        if (!user.Exists)
        {
            throw new BusinessException("User not exists in the system");
        }

        var passwordValid = _passwordEncryptor.Validate(credentials.Password, user.Credentials.Password);

        if (!passwordValid)
        {
            throw new BusinessException("Invalid password");
        }

        return user;
    }
}