using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPurchaseOrderService
    {
        Task<PageSourcePagination<PurchaseOrderRequestDto>> GetAllRequestsOrderToSuppliers(int pageNumber, string? medicineName = null, string? status = null, string? supplierName = null);
        Task RequestOrderToSupplier(Guid supplierId, Guid medicineId, int quantity);
        Task CompleteRequestedOrder(int orderId);
        Task DeclineRequestedOrder(int orderId);
    }
}
