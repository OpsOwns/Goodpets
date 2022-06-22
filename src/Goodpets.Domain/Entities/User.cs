namespace Goodpets.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    public UserId UserId { get; private set; }
    public Role Role { get; private set; }
    public Username Username { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public Token? Token { get; private set; }

    protected User()
    {
        UserId = null!;
        Role = null!;
        Username = null!;
        Password = null!;
        Email = null!;
        Token = null!;
    }

    public User(Role role, Username username, Password password, Email email)
    {
        UserId = new UserId();
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void ChangePassword(Password password)
    {
        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    public void ChangeToken(Token token)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    public void RemoveToken() => Token = null;
}