namespace Goodpets.Infrastructure.Abstractions;

public interface IEmailService
{
    Task Send(EmailMessage emailMessage, CancellationToken cancellationToken);
}