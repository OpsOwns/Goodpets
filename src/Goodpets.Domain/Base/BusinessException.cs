namespace Goodpets.Domain.Base;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}