namespace Goodpets.Infrastructure.Email;

[Serializable]
public class EmailException : Exception
{
    public EmailException() : base("Missing email configuration details")
    {
    }
}