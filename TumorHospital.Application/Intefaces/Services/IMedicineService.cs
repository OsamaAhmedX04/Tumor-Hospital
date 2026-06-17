using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Medicine;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IMedicineService
    {
        Task<PageSourcePagination<MedicineDto>> GetMedicines(int pageNumber, string? name = null);
        Task<MedicineDetailsDto> GetMedicineDetails(Guid medicineId);
        Task CreateMedicine(NewMedicineDto dto);
        Task UpdateMedicine(Guid medicineId, UpdateMedicineDto dto);
        Task DeleteMedicine(Guid medicineId);
        Task ReassignMedicineToSupplier(Guid medicineId, Guid supplierId);
        Task RequestOrderToSupplier(Guid SupplierId, Guid medicineId, int quantity);
        Task SellMedicine(Guid medicineId);

    }
}
