namespace Goodpets.Domain.Users.ValueObjects;

public class Credentials : ValueObject
{
    public string Username { get; }
    public string Password { get; }

    private Credentials(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public static Credentials Create(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        username = username.Trim();

        if (username.Length < 3)
            throw new BusinessException($"{nameof(username)} must be greater than 3 characters");


        return new Credentials(username, password);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return Password;
    }
}