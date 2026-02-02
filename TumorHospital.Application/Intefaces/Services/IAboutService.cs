using TumorHospital.Application.DTOs.Request.About_Contact;
using TumorHospital.Application.DTOs.Response.About_Contact;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAboutService
    {
        Task<AboutResponse> GetAboutAsync();
        Task AddOrUpdateAsync(AddAboutInfoDto dto);
        Task DeleteAsync(Guid id);
    }
}
