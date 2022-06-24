namespace Goodpets.Application.Abstractions;

public interface IEmailService
{
    Task Send(EmailMessage emailMessage, CancellationToken cancellationToken);
}