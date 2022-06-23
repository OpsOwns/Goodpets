namespace Goodpets.Infrastructure.Exceptions;

[Serializable]
public class OwnerException : BaseException
{
    public OwnerException(string message) : base(message)
    {
    }

    public OwnerException(string parameter, string message) : base(parameter, message)
    {
    }
}