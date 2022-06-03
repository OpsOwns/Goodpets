namespace Goodpets.Infrastructure.Security;

internal sealed class PasswordEncryptor : IPasswordEncryptor
{
    private readonly IPasswordHasher<UserAccount> _passwordHasher;

    public PasswordEncryptor(IPasswordHasher<UserAccount> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Encrypt(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        return _passwordHasher.HashPassword(default!, password);
    }

    public bool Validate(string password, string securedPassword)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        if (string.IsNullOrEmpty(securedPassword))
            throw new ArgumentNullException(nameof(securedPassword));

        return _passwordHasher.VerifyHashedPassword(default!, securedPassword, password) ==
               PasswordVerificationResult.Success;
    }
}