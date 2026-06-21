using LinqKit;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Request.Supply;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.Supply;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageSourcePagination<SupplierDto>> GetAllSuppliers(int pageNumber, string? name = null)
        {
            Expression<Func<Supplier, bool>> filter = s => !s.IsDeleted;

            if (!string.IsNullOrEmpty(name))
            {
                filter = filter.And(s => s.Name.Contains(name));
            }

            var result = await _unitOfWork.Suppliers.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Email = s.Email,
                    ContactPersonName = s.ContactPersonName,
                    ContactPersonPhone = s.ContactPersonPhone,
                    CreatedAt = s.CreatedAt,
                    PhoneNumber = s.PhoneNumber
                },
                pageNumber: pageNumber,
                pageSize: 20
                );

            return result;
        }

        public async Task CreateSupplier(NewSupplierDto dto)
        {
            var isNameExist = await _unitOfWork.Suppliers.AnyAsync(s => s.Name.ToLower() == dto.Name.ToLower());
            if (isNameExist) throw new Exception("This Name Is Already Exist");

            var newSupplier = new Supplier
            {
                Name = dto.Name,
                Address = dto.Address,
                ContactPersonName = dto.ContactPersonName,
                ContactPersonPhone = dto.ContactPersonPhone,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
            };

            await _unitOfWork.Suppliers.AddAsync(newSupplier);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSupplier(Guid supplierId)
        {
            var supplier = await _unitOfWork.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted);
            if (supplier is null) throw new Exception("Supplier Not Found");

            supplier.IsDeleted = true;

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateSupplier(Guid supplierId, UpdateSupplierDto dto)
        {
            var supplier = await _unitOfWork.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted);
            if (supplier is null) throw new Exception("Supplier Not Found");

            var isNameExist = await _unitOfWork.Suppliers.AnyAsync(s => s.Name.ToLower() == dto.Name.ToLower());
            if (isNameExist) throw new Exception("This Name Is Already Exist");

            supplier.Name = dto.Name;
            supplier.PhoneNumber = dto.PhoneNumber;
            supplier.Address = dto.Address;
            supplier.ContactPersonName = dto.ContactPersonName;
            supplier.ContactPersonPhone = dto.ContactPersonPhone;

            await _unitOfWork.CompleteAsync();
        }
    }
}
