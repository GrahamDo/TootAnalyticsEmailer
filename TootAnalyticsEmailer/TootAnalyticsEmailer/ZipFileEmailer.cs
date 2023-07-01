using System.Reflection.Metadata.Ecma335;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace TootAnalyticsEmailer;

internal class ZipFileEmailer
{
    private readonly Settings _settings;

    public ZipFileEmailer(Settings settings)
    {
        _settings = settings;
    }

    public void Send(string bodyText, string zipFileName)
    {
        if (string.IsNullOrEmpty(_settings.SmtpServer))
            throw new ApplicationException("Missing SMTP Server");
        if (_settings.SmtpPort <= 0)
            throw new ApplicationException("Missing or invalid SMTP Port");
        if (string.IsNullOrEmpty(_settings.EmailFromAddress))
            throw new ApplicationException("Missing email from address");
        if (string.IsNullOrEmpty(_settings.EmailFromDisplayName))
            throw new ApplicationException("Missing email from display name");
        if (string.IsNullOrEmpty(_settings.EmailSubject))
            throw new ApplicationException("Missing email subject");
        if (!_settings.ToEmailAddresses.Any())
            throw new ApplicationException("Missing to email addresses");

        using var smtpClient = new SmtpClient();
        smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
        smtpClient.Connect(_settings.SmtpServer, _settings.SmtpPort);

        if (!string.IsNullOrEmpty(_settings.SmtpUsername))
            smtpClient.Authenticate(_settings.SmtpUsername, _settings.SmtpPassword);

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_settings.EmailFromDisplayName, _settings.EmailFromAddress));
        foreach (var (address, displayName) in _settings.ToEmailAddresses)
            mailMessage.To.Add(new MailboxAddress(displayName, address));
        mailMessage.Subject = _settings.EmailSubject;

        var builder = new BodyBuilder { TextBody = bodyText };
        builder.Attachments.Add(zipFileName);
        mailMessage.Body = builder.ToMessageBody();

        smtpClient.Send(mailMessage);
        smtpClient.Disconnect(true);
    }
}