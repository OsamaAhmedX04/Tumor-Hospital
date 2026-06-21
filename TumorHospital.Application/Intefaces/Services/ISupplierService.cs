using TumorHospital.Application.DTOs.Request.Supply;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.Supply;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface ISupplierService
    {
        Task<PageSourcePagination<SupplierDto>> GetAllSuppliers(int pageNumber, string? name = null);
        Task CreateSupplier(NewSupplierDto dto);
        Task UpdateSupplier(Guid supplierId, UpdateSupplierDto dto);
        Task DeleteSupplier(Guid supplierId);
    }
}
