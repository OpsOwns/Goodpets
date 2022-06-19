using Goodpets.Application.Abstractions;
using Goodpets.Application.Abstractions.Email;

namespace Goodpets.Infrastructure.Services;

internal class EmailService : IEmailService
{
    private const string ServiceName = "Goodpets";
    private readonly EmailOptions _options;

    public EmailService(EmailOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task Send(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        if (emailMessage is null)
            throw new EmailException();

        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(ServiceName, _options.From));
        mimeMessage.To.Add(new MailboxAddress(ServiceName, emailMessage.Receiver));
        mimeMessage.Subject = emailMessage.Subject;

        mimeMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = emailMessage.Body
        };

        using var client = new SmtpClient
        {
            CheckCertificateRevocation = false
        };

        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);

        await client.SendAsync(mimeMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
    
}