using TumorHospital.Application.DTOs.Response.About_Contact;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAboutService
    {
        Task<AboutResponse> GetAboutAsync();
    }
}
