namespace Goodpets.Domain.SeedWork;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}