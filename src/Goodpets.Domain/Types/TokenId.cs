using Goodpets.Domain.Base;

namespace Goodpets.Domain.Types;

public record TokenId : EntityId
{
    public TokenId(Guid value) : base(value)
    {
    }

    public TokenId()
    {
    }
}