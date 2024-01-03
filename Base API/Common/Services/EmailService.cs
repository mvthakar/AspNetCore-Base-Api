using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

using BaseAPI.Common.Settings;

namespace BaseAPI.Common.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings emailSettings = emailSettings.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using (var client = new SmtpClient(emailSettings.SmtpHost, emailSettings.SmtpPort))
        {
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(emailSettings.Email, emailSettings.Password);

            using (var mail = new MailMessage(emailSettings.Email, to))
            {
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;

                await client.SendMailAsync(mail);
            };
        };
    }
}