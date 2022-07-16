using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Demo.Infrastructure.Email;

internal class EmailSender : IEmailSender
{
    private readonly EnvironmentSettings _environmentSettings;

    public EmailSender(EnvironmentSettings environmentSettings)
    {
        _environmentSettings = environmentSettings;
    }

    public async Task SendAsync(string toAddress, string subject, string htmlContent,
        CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_environmentSettings.Email.FromAddress));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_environmentSettings.Email.Host, _environmentSettings.Email.Port,
            SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
        if (!string.IsNullOrEmpty(_environmentSettings.Email.Username))
        {
            await smtpClient.AuthenticateAsync(_environmentSettings.Email.Username,
                _environmentSettings.Email.Password, cancellationToken);
        }

        await smtpClient.SendAsync(message, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}
