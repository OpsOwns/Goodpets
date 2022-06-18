namespace Goodpets.Infrastructure.Email;

public record EmailMessage(string Body, string Receiver, string Subject);
