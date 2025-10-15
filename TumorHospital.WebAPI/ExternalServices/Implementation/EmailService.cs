using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TumorHospital.WebAPI.ExternalServices.Interfaces;
using TumorHospital.WebAPI.Settings;

namespace TumorHospital.WebAPI.ExternalServices.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly SMTPSettings _smtp;
        public EmailService(IOptions<SMTPSettings> smtpOptions)
        {
            _smtp = smtpOptions.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            message.From = new MailAddress(_smtp.UserName, _smtp.DisplayName);

            using var smtpClient = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                Credentials = new NetworkCredential(_smtp.UserName, _smtp.Password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
