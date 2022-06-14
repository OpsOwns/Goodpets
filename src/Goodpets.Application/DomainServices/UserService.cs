namespace Goodpets.Application.DomainServices;

public class UserService : IUserService
{
    private readonly IUserAccountRepository _repository;
    private readonly IPasswordEncryptor _passwordEncryptor;

    public UserService(IUserAccountRepository repository, IPasswordEncryptor passwordEncryptor)
    {
        _repository = repository;
        _passwordEncryptor = passwordEncryptor;
    }


    public async Task<Result<UserAccount>> RegisterUser(string username, string password, string email,
        CancellationToken cancellationToken)
    {
        var credentials = Credentials.Create(username, _passwordEncryptor.Encrypt(password));
        var userEmail = Email.Create(email);

        var resultConcat = Result.Merge(credentials.ToResult(), userEmail.ToResult());

        if (resultConcat.IsFailed)
            return resultConcat;

        if (await _repository.CheckUserExistsByEmail(userEmail.Value, cancellationToken))
        {
            throw new BusinessException($"User with email {userEmail.Value} already exists in system");
        }

        return Result.Ok(new UserAccount(Role.User(), credentials.Value, userEmail.Value));
    }

    public async Task<Result<UserAccount>> Login(string username, string password, CancellationToken cancellationToken)
    {
        var credentials = Credentials.Create(username, password);

        var userResult = await _repository.GetUserAccount(credentials.Value.Username, cancellationToken);

        if (userResult.IsFailed)
        {
            return Result.Fail(new Error("User not exists"));
        }

        var passwordValid =
            _passwordEncryptor.Validate(credentials.Value.Password, userResult.Value.Credentials.Password);

        if (!passwordValid)
        {
            return Result.Fail("Invalid password");
        }

        return Result.Ok(userResult.Value);
    }
}