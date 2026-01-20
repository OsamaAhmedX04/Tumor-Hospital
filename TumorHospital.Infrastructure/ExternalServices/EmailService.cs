using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Text.RegularExpressions;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Infrastructure.Settings;

namespace TumorHospital.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _settings;
        public EmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.EmailSender, "Tumor Hospital");
            var to = new EmailAddress(toEmail, "user");
            var plainTextContent = HtmlToPlainTextPreserveLinks(body);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, body);
            await client.SendEmailAsync(msg);
        }


        private static string HtmlToPlainTextPreserveLinks(string html)
        {
            // Convert links to: text (url)
            html = Regex.Replace(
                html,
                "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\"[^>]*>(.*?)</a>",
                "$2 ($1)",
                RegexOptions.IgnoreCase);

            // Remove remaining HTML tags
            html = Regex.Replace(html, "<.*?>", string.Empty);

            return WebUtility.HtmlDecode(html).Trim();
        }

    }
}
