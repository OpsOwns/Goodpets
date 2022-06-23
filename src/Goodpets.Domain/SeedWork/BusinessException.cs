namespace Goodpets.Domain.SeedWork;

[Serializable]
public class BusinessException : BaseException
{
    public BusinessException(string message) : base(message)
    {
    }
}