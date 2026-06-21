using LinqKit;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Request.Medicine;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public MedicineService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PageSourcePagination<MedicineDto>> GetMedicines(int pageNumber, string? name = null)
        {
            Expression<Func<Medicine, bool>> filter = m => m.IsActive;

            if (!string.IsNullOrEmpty(name))
            {
                filter = filter.And(m => m.Name.Contains(name) || m.Description.Contains(name));
            }

            var result = await _unitOfWork.Medicines.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: m => new MedicineDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    CreatedAt = m.CreatedAt,
                    SellingPrice = m.SellingPrice,
                    PurchasePrice = m.PurchasePrice,
                    QuantityInStock = m.QuantityInStock,
                    MinimumQuantity = m.MinimumQuantity,
                    SupplierId = m.SupplierId,
                    SupplierName = m.Supplier.Name
                },
                pageNumber: pageNumber,
                pageSize: 25
                );

            return result;
        }

        public async Task CreateMedicine(NewMedicineDto dto)
        {
            var isNameExist = await _unitOfWork.Medicines.AnyAsync(m => m.Name.ToLower() == dto.Name.ToLower());
            if (isNameExist) throw new Exception("This Medicine Name Is Already Exist");

            var currentPharmacistId = _currentUserService.UserId;

            var newMedicine = new Medicine
            {
                Name = dto.Name,
                CreatedByPharmacistId = currentPharmacistId,
                SupplierId = dto.SupplierId,
                Description = dto.Description,
                MinimumQuantity = dto.MinimumQuantity,
                SellingPrice = dto.SellingPrice,
                PurchasePrice = dto.PurchasePrice,
                QuantityInStock = dto.QuantityInStock
            };

            await _unitOfWork.Medicines.AddAsync(newMedicine);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteMedicine(Guid medicineId)
        {
            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == medicineId && m.IsActive);
            if (medicine is null) throw new Exception("Medicine Not Found");

            medicine.IsActive = false;
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateMedicine(Guid medicineId, UpdateMedicineDto dto)
        {
            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == medicineId && m.IsActive);
            if (medicine is null) throw new Exception("Medicine Not Found");

            var isNameExist = await _unitOfWork.Medicines.AnyAsync(m => m.Name.ToLower() == dto.Name.ToLower());
            if (isNameExist) throw new Exception("This Medicine Name Is Already Exist");

            var currentPharmacistId = _currentUserService.UserId;

            medicine.Name = dto.Name;
            medicine.Description = dto.Description;
            medicine.SellingPrice = dto.SellingPrice;
            medicine.PurchasePrice = dto.PurchasePrice;
            medicine.QuantityInStock = dto.QuantityInStock;
            medicine.MinimumQuantity = dto.MinimumQuantity;
            medicine.CreatedByPharmacistId = currentPharmacistId;

            await _unitOfWork.CompleteAsync();
        }

        public async Task ReassignMedicineToSupplier(Guid medicineId, Guid supplierId)
        {
            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == medicineId && m.IsActive);
            if (medicine is null) throw new Exception("Medicine Not Found");

            var supplier = await _unitOfWork.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted);
            if (supplier is null) throw new Exception("Supplier Not Found");

            medicine.SupplierId = supplierId;

            await _unitOfWork.CompleteAsync();
        }

        public async Task SellMedicine(Guid medicineId, int quantity)
        {
            var medicine = await _unitOfWork.Medicines.FirstOrDefaultAsync(m => m.Id == medicineId && m.IsActive);
            if (medicine is null) throw new Exception("Medicine Not Found");

            var currentPharmacistId = _currentUserService.UserId;

            if (quantity > medicine.QuantityInStock) throw new Exception("Insufficient quantity in stock");

            medicine.QuantityInStock -= quantity;

            var pharmacy = await _unitOfWork.Pharmacists.GetEnhancedAsync(
                filter: p => p.ApplicationUserId == currentPharmacistId,
                selector: p => new { p.PharmacyId }
                );

            var medicineSale = new MedicineSale
            {
                MedicineId = medicineId,
                PharmacyId = pharmacy!.PharmacyId,
                Quantity = quantity,
                SoldByPharmacistId = currentPharmacistId,
                TotalAmount = quantity * medicine.SellingPrice,
                UnitPrice = medicine.SellingPrice,
            };

            await _unitOfWork.MedicineSales.AddAsync(medicineSale);
            await _unitOfWork.CompleteAsync();
        }
    }
}
