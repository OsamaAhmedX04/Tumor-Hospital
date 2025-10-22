namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
