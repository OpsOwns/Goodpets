namespace Goodpets.Domain.Users.Entities;

public sealed class UserAccount : Entity<UserAccountId>
{
    private UserAccount()
    {
    }

    public UserAccount(Role role, Credentials credentials, Email email) : base(new UserAccountId())
    {
        Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }

    public Role Role { get; } = null!;
    public Credentials Credentials { get; } = null!;
    public Email Email { get; } = null!;
}