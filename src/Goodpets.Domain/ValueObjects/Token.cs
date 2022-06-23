namespace Goodpets.Domain.ValueObjects;

public class Token : ValueObject
{
    public string RefreshToken { get; }
    public LocalDateTime ExpireDate { get; }
    public LocalDateTime CreationDate { get; }
    public bool Used { get; }
    public JwtId JwtId { get; }

    private Token()
    {
        RefreshToken = null!;
        JwtId = null!;
    }

    private Token(string refreshToken, LocalDateTime expireDate, bool used,
        JwtId jwtId) : this()
    {
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
        CreationDate = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime;
        Used = used;
        JwtId = jwtId;
    }

    public static Result<Token> Create(string refreshToken, LocalDateTime expireDate,
        bool used,
        JwtId jwtId)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result.Fail(
                ErrorResultMessages.NotNullOrEmptyError(nameof(refreshToken)));

        if (jwtId.Value == Guid.Empty)
        {
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(jwtId)));
        }

        return Result.Ok(new Token(refreshToken, expireDate, used, jwtId));
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