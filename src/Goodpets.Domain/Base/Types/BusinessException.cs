namespace Goodpets.Domain.Base.Types;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}