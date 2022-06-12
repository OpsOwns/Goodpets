namespace Goodpets.Domain.Users.Entities;

public sealed class Token : Entity<TokenId>
{
    public string RefreshToken { get; private set; } = null!;
    public DateTime ExpireDate { get; private set; }
    public DateTime CreationDate { get; private set; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }
    public UserAccountId UserAccountId { get; private set; } = null!;
    public JwtId JwtId { get; private set; } = null!;

    private Token()
    {
    }

    private Token(string refreshToken, UserAccountId userAccountId, JwtId jwtId, DateTime expireDate,
        DateTime creationDate,
        bool used, bool invalidated) :
        base(new TokenId())
    {
        RefreshToken = refreshToken;
        UserAccountId = userAccountId;
        JwtId = jwtId;
        ExpireDate = expireDate;
        CreationDate = creationDate;
        Used = used;
        Invalidated = invalidated;
    }

    public static Result<Token> Create(string token, DateTime expireDate, DateTime creationDate,
        UserAccountId userAccountId, JwtId jwtId, bool invalidated,
        bool used)
    {
        if (userAccountId == null)
            throw new ArgumentNullException(nameof(userAccountId));

        if (jwtId == null)
            throw new ArgumentNullException(nameof(jwtId));

        if (string.IsNullOrWhiteSpace(token))
            return Result.Fail("Token can't be null or empty");

        if (expireDate < DateTime.Now)
            return Result.Fail("Expire date can't be less than current time");

        return Result.Ok(new Token(token, userAccountId, jwtId, expireDate, creationDate, invalidated, used));
    }

    public void ChangeStatus(bool used)
    {
        Used = used;
    }
}