namespace Goodpets.Infrastructure.Email.Exceptions;

[Serializable]
public class EmailException : Exception
{
    public EmailException() : base("Missing email configuration details")
    {
    }
}