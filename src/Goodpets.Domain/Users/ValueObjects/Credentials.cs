using Goodpets.Domain.Base.Types;

namespace Goodpets.Domain.Users.ValueObjects;

public class Credentials : ValueObject
{
    private Credentials(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; }
    public string Password { get; }

    public static Result<Credentials> Create(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
            Result.Fail("username can't be empty");

        if (string.IsNullOrEmpty(password))
            Result.Fail("password can't be empty");

        username = username.Trim();

        if (username.Length < 3)
            return Result.Fail($"{nameof(username)} must be greater than 3 characters");


        return Result.Ok(new Credentials(username, password));
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return Password;
    }
}