namespace Goodpets.Application.Abstractions.Email;

public interface IEmailService
{
    Task Send(EmailMessage emailMessage, CancellationToken cancellationToken);
}