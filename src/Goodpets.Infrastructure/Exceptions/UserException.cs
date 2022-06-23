namespace Goodpets.Infrastructure.Exceptions;

[Serializable]
public class UserException : BaseException
{
    public UserException(string parameter, string message) : base(parameter, message)
    {
    }

    public UserException(string message) : base(message)
    {
    }
}