namespace Goodpets.Domain.Entities;

public sealed class User : Entity, IAggregateRoot
{
    public UserId UserId { get; private set; }
    public Role Role { get; private set; }
    public Username Username { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public Token? Token { get; private set; }

    private User()
    {
        UserId = null!;
        Role = null!;
        Username = null!;
        Password = null!;
        Email = null!;
        Token = null!;
    }


    private User(Role role, Username username, Password password, Email email)
    {
        UserId = new UserId();
        Role = role;
        Username = username;
        Password = password;
        Email = email;
    }

    public static Result<User> Create(Role? role, Username? username, Password? password, Email? email)
    {
        return role is null ? Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(role))) :
            username is null ? Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(username))) :
            password is null ? Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(password))) :
            email is null ? Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(email))) :
            Result.Ok(new User(role, username, password, email));
    }

    public Result ChangePassword(Password? password)
    {
        if (password is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(Password)));

        Password = password;

        return Result.Ok();
    }

    public Result ChangeToken(Token? token)
    {
        if (token is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(Password)));

        Token = token;

        return Result.Ok();
    }

    public void RemoveToken() => Token = null;
}