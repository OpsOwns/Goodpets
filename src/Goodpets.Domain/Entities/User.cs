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

    public User(Role role, Username username, Password password, Email email, Token? token = null)
    {
        UserId = new UserId();
        Role = role;
        Username = username;
        Password = password;
        Email = email;
        Token = token;
    }

    public void ChangePassword(Password password)
    {
        if (password == null)
            throw new BusinessException("Password can't be null");

        Password = password;
    }

    public void ChangeToken(Token token)
    {
        if (token == null) throw new BusinessException("Token can't be null");
        Token = token;
    }

    public void RemoveToken() => Token = null;
}