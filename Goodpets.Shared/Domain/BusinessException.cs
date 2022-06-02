namespace Goodpets.Shared.Domain;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}