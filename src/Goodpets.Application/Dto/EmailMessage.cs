namespace Goodpets.Infrastructure.Email.Models;

public record EmailMessage(string Body, string Receiver, string Subject);
