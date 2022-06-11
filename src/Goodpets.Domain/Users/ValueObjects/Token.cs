namespace Goodpets.Domain.Users.ValueObjects;

public class Token : ValueObject
{
    public string? RefreshToken { get; }
    public DateTime? ExpireDate { get; }
    public DateTime? CreationDate { get; }
    public bool? Used { get; }
    public bool? Invalidated { get; }

    private Token(string refreshToken, DateTime expireDate, DateTime creationDate, bool? used, bool? invalidated)
    {
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
        CreationDate = creationDate;
        Used = used;
        Invalidated = invalidated;
    }

    public static Result<Token> Create(string token, DateTime expireDate, bool? invalidated, bool? used)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Fail("Token can't be null or empty");

        if (expireDate < DateTime.Now)
            return Result.Fail("Expire date can't be less than current time");

        return Result.Ok(new Token(token, expireDate, DateTime.UtcNow, used, invalidated));
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