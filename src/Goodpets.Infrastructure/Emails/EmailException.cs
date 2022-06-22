namespace Goodpets.Infrastructure.Emails;

[Serializable]
public class EmailException : Exception
{
    public EmailException() : base("Missing email configuration details")
    {
    }
}