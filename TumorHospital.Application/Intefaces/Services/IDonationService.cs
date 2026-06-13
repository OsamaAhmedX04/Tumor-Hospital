using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Request.Payment;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IDonationService
    {
        Task Donate(VolunteerDto dto);
        Task<string> CreateDonation(VolunteerDto dto);
        Task HandleWebhook([FromBody] WebHookModel model);
        Task<decimal> SuccessDonation(string invoiceId);
        Task FaildedDonation(string invoiceId);
    }
}
