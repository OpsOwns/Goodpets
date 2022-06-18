﻿namespace Goodpets.Infrastructure.Email;

internal sealed class EmailOptions
{
    private const string SectionName = "EmailBox";
    internal string From { get; init; }
    internal string SmtpServer { get; init; }
    internal string Username { get; init; }
    internal string Password { get; init; }
    internal int Port { get; init; }

    internal EmailOptions(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var section = configuration.GetSection(SectionName);

        if (section is null) throw new InvalidOperationException("Can't find configuration section");

        From = section.GetValue<string>(nameof(From));
        SmtpServer = section.GetValue<string>(nameof(SmtpServer));
        Username = section.GetValue<string>(nameof(Username));
        Password = section.GetValue<string>(nameof(Password));
        Port = section.GetValue<int>(nameof(Port));
    }
}