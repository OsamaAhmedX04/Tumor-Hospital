using TumorHospital.Application.DTOs.Request.Medicine;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IMedicineService
    {
        Task<PageSourcePagination<MedicineDto>> GetMedicines(int pageNumber, string? name = null);
        Task CreateMedicine(NewMedicineDto dto);
        Task UpdateMedicine(Guid medicineId, UpdateMedicineDto dto);
        Task DeleteMedicine(Guid medicineId);
        Task ReassignMedicineToSupplier(Guid medicineId, Guid supplierId);
        Task SellMedicine(Guid medicineId, int quantity);

    }
}
