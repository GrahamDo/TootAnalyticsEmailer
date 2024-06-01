using System.Reflection.Metadata.Ecma335;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace TootAnalyticsEmailer;

internal class ZipFileEmailer(Settings mainSettings)
{
    public void Send(string bodyText, string zipFileName)
    {
        if (string.IsNullOrEmpty(mainSettings.SmtpServer))
            throw new ApplicationException("Missing SMTP Server");
        if (mainSettings.SmtpPort <= 0)
            throw new ApplicationException("Missing or invalid SMTP Port");
        if (string.IsNullOrEmpty(mainSettings.EmailFromAddress))
            throw new ApplicationException("Missing email from address");
        if (string.IsNullOrEmpty(mainSettings.EmailFromDisplayName))
            throw new ApplicationException("Missing email from display name");
        if (string.IsNullOrEmpty(mainSettings.EmailSubject))
            throw new ApplicationException("Missing email subject");
        if (!mainSettings.ToEmailAddresses.Any())
            throw new ApplicationException("Missing to email addresses");

        using var smtpClient = new SmtpClient();
        smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
        smtpClient.Connect(mainSettings.SmtpServer, mainSettings.SmtpPort);

        if (!string.IsNullOrEmpty(mainSettings.SmtpUsername))
            smtpClient.Authenticate(mainSettings.SmtpUsername, mainSettings.SmtpPassword);

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(mainSettings.EmailFromDisplayName, mainSettings.EmailFromAddress));
        foreach (var (address, displayName) in mainSettings.ToEmailAddresses)
            mailMessage.To.Add(new MailboxAddress(displayName, address));
        mailMessage.Subject = mainSettings.EmailSubject;

        var builder = new BodyBuilder { TextBody = bodyText };
        builder.Attachments.Add(zipFileName);
        mailMessage.Body = builder.ToMessageBody();

        smtpClient.Send(mailMessage);
        smtpClient.Disconnect(true);
    }
}