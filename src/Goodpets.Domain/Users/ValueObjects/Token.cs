namespace Goodpets.Domain.Users.ValueObjects;

public class Token : ValueObject
{
    public string? RefreshToken { get; }
    public DateTime? ExpireDate { get; }
    public DateTime? CreationDate { get; }
    public bool? Used { get; }
    public bool? Invalidated { get; }

    public static Token Empty => new();

    private Token()
    {
    }

    private Token(string refreshToken, DateTime expireDate)
    {
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
        Used = false;
        Invalidated = false;
    }

    private Token(string refreshToken, DateTime expireDate, DateTime creationDate, bool used, bool invalidated)
    {
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
        CreationDate = creationDate;
        Used = used;
        Invalidated = invalidated;
    }

    public Token Create(string token, DateTime expireDate, bool invalidated, bool used)
    {
        Validate(token, expireDate);

        return new(token, expireDate, DateTime.UtcNow, used, invalidated);
    }

    public Token Refresh(string token, DateTime expireTime)
    {
        Validate(token, expireTime);

        return new(token, expireTime);
    }

    private static void Validate(string token, DateTime expireDate)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new BusinessException("Token can't be null or empty");

        if (expireDate < DateTime.Now)
            throw new BusinessException("Expire date can't be less than current time");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RefreshToken!;
        yield return ExpireDate!;
        yield return CreationDate!;
        yield return Used!;
        yield return Invalidated!;
    }
}