using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.Interfaces.ExternalServices;
using TumorHospital.Application.Settings;

namespace TumorHospital.Infrastructure.ExternalServices
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
