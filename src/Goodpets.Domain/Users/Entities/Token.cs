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

    public static Token Create(string token, DateTime expireDate, DateTime creationDate,
        UserAccountId userAccountId, JwtId jwtId, bool used)
    {
        if (userAccountId == null)
            throw new BusinessException($"{nameof(userAccountId)} can't be null");

        if (jwtId == null)
            throw new BusinessException($"{nameof(jwtId)} can't be null");

        if (string.IsNullOrWhiteSpace(token))
            throw new BusinessException($"{nameof(token)} can't be null");

        return new Token(token, userAccountId, jwtId, expireDate, creationDate, used);
    }

    public void UseRefreshToken()
    {
        Used = true;
    }
}