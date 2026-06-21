using LinqKit;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public PurchaseOrderService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PageSourcePagination<PurchaseOrderRequestDto>>
            GetAllRequestsOrderToSuppliers(int pageNumber, string? medicineName = null, string? status = null, string? supplierName = null)
        {
            Expression<Func<MedicinePurchaseOrder, bool>> filter = o => true;

            if (!string.IsNullOrEmpty(medicineName))
            {
                filter = filter.And(o => o.Medicine!.Name.Contains(medicineName));
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                filter = filter.And(o => o.Supplier!.Name.Contains(supplierName));
            }

            if (!string.IsNullOrEmpty(status))
            {
                filter = filter.And(o => o.Status.ToString().ToLower() == status.ToLower());
            }

            var result = await _unitOfWork.MedicinePurchaseOrders.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: o => new PurchaseOrderRequestDto
                {
                    CreatedAt = o.CreatedAt,
                    CreatedByPharmacistName = o.CreatedByPharmacist.User.FirstName + " " + o.CreatedByPharmacist.User.LastName,
                    CreatedByPharmacistPhoneNumber = o.CreatedByPharmacist.User.PhoneNumber,
                    MedicineName = o.Medicine.Name,
                    PharmacyLocation = o.Pharmacy.Location,
                    PharmacyName = o.Pharmacy.Name,
                    Quantity = o.Quantity,
                    Status = o.Status.ToString(),
                    SupplierName = o.Supplier.Name,
                    SupplierPhoneNumber = o.Supplier.PhoneNumber,
                    TotalAmount = o.TotalAmount,
                    UnitPrice = o.UnitPrice
                },
                orderBy: o => o.OrderByDescending(p => p.CreatedAt),
                pageNumber: pageNumber,
                pageSize: 20
                );

            return result;
        }

        public async Task RequestOrderToSupplier(Guid supplierId, Guid medicineId, int quantity)
        {
            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == medicineId && m.IsActive);
            if (medicine is null) throw new Exception("Medicine Not Found");

            var isSupplierExist = await _unitOfWork.Suppliers.AnyAsync(s => s.Id == supplierId && !s.IsDeleted);
            if (!isSupplierExist) throw new Exception("Supplier Not Found");

            var currentPharmacistId = _currentUserService.UserId;
            var pharmacy = await _unitOfWork.Pharmacists.GetEnhancedAsync(
                filter: p => p.ApplicationUserId == currentPharmacistId,
                selector: p => new { p.PharmacyId }
                );

            var order = new MedicinePurchaseOrder
            {
                PharmacyId = pharmacy.PharmacyId,
                CreatedByPharmacistId = currentPharmacistId,
                SupplierId = supplierId,
                MedicineId = medicineId,
                Quantity = quantity,
                Status = PurchaseStatus.Pending,
                UnitPrice = medicine.PurchasePrice,
                TotalAmount = quantity * medicine.PurchasePrice
            };

            await _unitOfWork.MedicinePurchaseOrders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CompleteRequestedOrder(int orderId)
        {
            var order = await _unitOfWork.MedicinePurchaseOrders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.Status == PurchaseStatus.Pending);
            if (order is null) throw new Exception("Order Not Found Or Not Pending");

            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == order.MedicineId);
            if (medicine is null) throw new Exception("medicine Not Found");

            order.Status = PurchaseStatus.Completed;
            medicine.QuantityInStock += order.Quantity;

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeclineRequestedOrder(int orderId)
        {
            var order = await _unitOfWork.MedicinePurchaseOrders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.Status == PurchaseStatus.Pending);
            if (order is null) throw new Exception("Order Not Found Or Not Pending");

            order.Status = PurchaseStatus.Declined;

            await _unitOfWork.CompleteAsync();
        }


    }
}
