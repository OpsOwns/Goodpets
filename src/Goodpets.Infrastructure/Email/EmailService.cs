using Goodpets.Infrastructure.Email.Exceptions;
using Goodpets.Infrastructure.Email.Options;

namespace Goodpets.Infrastructure.Email;

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

        using var client = new SmtpClient();

        await client.ConnectAsync(_options.SmtpServer, _options.Port, SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);

        await client.SendAsync(mimeMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}