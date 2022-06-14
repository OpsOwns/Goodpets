using Goodpets.Domain.Base.Types;

namespace Goodpets.Domain.Users.Entities;

public sealed class Token : Entity<TokenId>
{
    private Token()
    {
    }

    private Token(string refreshToken, UserAccountId userAccountId, JwtId jwtId, DateTime expireDate,
        DateTime creationDate, bool used) : base(new TokenId())
    {
        RefreshToken = refreshToken;
        UserAccountId = userAccountId;
        JwtId = jwtId;
        ExpireDate = expireDate;
        CreationDate = creationDate;
        Used = used;
    }

    public string RefreshToken { get; } = null!;
    public DateTime ExpireDate { get; }
    public DateTime CreationDate { get; }
    public bool Used { get; private set; }
    public UserAccountId UserAccountId { get; } = null!;
    public JwtId JwtId { get; } = null!;

    public static Result<Token> Create(string token, DateTime expireDate, DateTime creationDate,
        UserAccountId userAccountId, JwtId jwtId, bool used)
    {
        if (userAccountId == null)
            throw new ArgumentNullException(nameof(userAccountId));

        if (jwtId == null)
            throw new ArgumentNullException(nameof(jwtId));

        if (string.IsNullOrWhiteSpace(token))
            return Result.Fail("Token can't be null or empty");

        return Result.Ok(new Token(token, userAccountId, jwtId, expireDate, creationDate, used));
    }

    public void UseRefreshToken()
    {
        Used = true;
    }
}