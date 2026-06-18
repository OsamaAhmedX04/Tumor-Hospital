using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPurchaseOrderService
    {
        Task<PageSourcePagination<PurchaseOrderRequestDto>> RequestOrderToSupplier(int pageNumber, string? medicineName = null, string? status = null, string? supplierName = null);
        Task RequestOrderToSupplier(Guid SupplierId, Guid medicineId, int quantity);
        Task CompleteRequestedOrder(int orderId);
        Task DeclineRequestedOrder(int orderId);
    }
}
