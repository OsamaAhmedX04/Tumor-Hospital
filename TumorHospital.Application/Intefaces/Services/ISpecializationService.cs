using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface ISpecializationService
    {
        Task AddSpecialization(SpecializationDto model);
        Task UpdateSpecialization(Guid id, SpecializationDto model);
        Task DeleteSpecialization(Guid id);
        Task<List<SpecializationDetailsDto>> GetSpecializations();
    }
}
