using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace COMP2139_ICE.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtp = _config.GetSection("SmtpSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Project Management App", smtp["Username"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                smtp["Host"],
                int.Parse(smtp["Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(smtp["Username"], smtp["Password"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}