using TumorHospital.Application.DTOs.Request.Donation;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IDonationService
    {
        Task Donate(VolunteerDto volunteer);

    }
}
