namespace Goodpets.Domain.Users;

public class UserAccount : Entity<UserAccountId>
{
    public Role Role { get; private set; } = null!;
    public Credentials Credentials { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Token? RefreshToken { get; private set; }

    public static UserAccount NotFound() => new();

    private UserAccount()
    {
    }

    public UserAccount(Role role, Credentials credentials, Email email) : base(new UserAccountId())
    {
        Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }

    public void CreateRefreshToken(Token refreshToken)
    {
        if (refreshToken == null) throw new ArgumentNullException(nameof(refreshToken));

        RefreshToken = refreshToken;
    }
}