namespace Goodpets.Infrastructure.Exceptions;

[Serializable]
public class EmailException : BaseException
{
    public EmailException() : base("Missing email configuration details")
    {
    }
}