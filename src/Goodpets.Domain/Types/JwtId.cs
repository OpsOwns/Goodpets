namespace Goodpets.Domain.Types;

public record JwtId : EntityId
{
    public JwtId(Guid value) : base(value)
    {
    }

    public JwtId()
    {
    }

    public static implicit operator JwtId(string jwtId)
    {
        if (string.IsNullOrEmpty(jwtId))
            throw new ArgumentNullException(nameof(jwtId));

        if (!Guid.TryParse(jwtId, out var jwtIdParsed))
        {
            throw new InvalidCastException(nameof(jwtId));
        }

        return new JwtId(jwtIdParsed);
    }
}