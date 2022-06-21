namespace Goodpets.Domain.ValueObjects;

public class Token : ValueObject
{
    public string RefreshToken { get; }
    public DateTime ExpireDate { get; }
    public DateTime CreationDate { get; }
    public bool Used { get; }
    public JwtId JwtId { get; }

    protected Token()
    {
        RefreshToken = null!;
        JwtId = null!;
    }

    private Token(string refreshToken, DateTime expireDate, DateTime creationDate, bool used, JwtId jwtId) : this()
    {
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
        CreationDate = creationDate;
        Used = used;
        JwtId = jwtId;
    }

    public static Result<Token> Create(string refreshToken, DateTime expireDate, DateTime creationDate, bool used,
        JwtId jwtId)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result.Fail(
                new Error("refreshToken can't be null or empty")
                    .WithMetadata("ErrorParameter", "RefreshToken"));

        if (jwtId.Value == Guid.Empty)
        {
            return Result.Fail(
                new Error("jwtId can't be empty")
                    .WithMetadata("ErrorParameter", "JwtId"));
        }

        return Result.Ok(new Token(refreshToken, expireDate, creationDate, used, jwtId));
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RefreshToken;
        yield return ExpireDate;
        yield return CreationDate;
        yield return Used;
        yield return JwtId;
    }
}