namespace Goodpets.Domain.Users.Entities;

public sealed class UserAccount : Entity<UserAccountId>
{
    private UserAccount()
    {
    }

    public UserAccount(Role role, Credentials credentials, Email email) : base(new UserAccountId())
    {
        Credentials = credentials ?? throw new BusinessException($"{nameof(credentials)} can't be null");
        Email = email ?? throw new BusinessException($"{nameof(email)} can't be null");
        Role = role ?? throw new BusinessException($"{nameof(role)} can't be null");
    }

    public Role Role { get; } = null!;
    public Credentials Credentials { get; } = null!;
    public Email Email { get; } = null!;
}