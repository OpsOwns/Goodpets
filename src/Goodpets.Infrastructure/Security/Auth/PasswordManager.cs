namespace Goodpets.Infrastructure.Security.Auth;

internal sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordManager(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string GeneratePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var builder = new StringBuilder();
        var random = new Random();

        while (0 < length--)
        {
            builder.Append(valid[random.Next(valid.Length)]);
        }

        return builder.ToString();
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