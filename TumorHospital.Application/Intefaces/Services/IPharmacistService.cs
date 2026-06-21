using TumorHospital.Application.DTOs.Request.Pharmacy;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPharmacistService
    {
        Task CreatePharmacist(NewPharmacistDto dto);
        Task DeletePharmacist(string pharmacistId);
    }
}
