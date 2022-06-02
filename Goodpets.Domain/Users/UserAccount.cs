namespace Goodpets.Domain.Users;

public class UserAccount : Entity<UserAccountId>
{
    public Role Role { get; private set; }
    public Credentials Credentials { get; private set; }
    public Email Email { get; private set; }

    public static UserAccount NotFound() => new();

    private UserAccount()
    {
    }

    public UserAccount(UserAccountId id, Role role, Credentials credentials, Email email) : base(id)
    {
        Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }
}