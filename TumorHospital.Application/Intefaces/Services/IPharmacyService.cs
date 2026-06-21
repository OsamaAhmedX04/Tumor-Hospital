using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Pharmacy;
using TumorHospital.Application.DTOs.Response.Pharmacy;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPharmacyService
    {
        Task<List<PharmacyDto>> GetAllPharmacies();
        Task<PharmacyDetailsDto> GetPharmacy(Guid pharmacyId, int? year = null, int? month = null);
        Task CreatePharmacy(NewPharmacyDto dto);
        Task UpdatePharmacy(Guid pharmacyId, UpdatePharmacyDto dto);
        Task DeletePharmacy(Guid pharmacyId);
    }
}
