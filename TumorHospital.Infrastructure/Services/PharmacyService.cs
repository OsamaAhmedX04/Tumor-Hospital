using LinqKit;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Request.Pharmacy;
using TumorHospital.Application.DTOs.Response.Pharmacy;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PharmacyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PharmacyDto>> GetAllPharmacies()
        {
            var result = await _unitOfWork.Pharmacies.GetAllAsyncEnhanced(
                filter: p => !p.IsActive,
                selector: p => new PharmacyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Location = p.Location,
                    PhoneNumber = p.PhoneNumber,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt
                }
                );

            return result;
        }

        public async Task<PharmacyDetailsDto> GetPharmacy(Guid pharmacyId, int? year = null, int? month = null)
        {
            Expression<Func<MedicineSale, bool>> filter = m => true;

            if (year != null)
            {
                filter = filter.And(m => m.PharmacyId == pharmacyId && m.CreatedAt!.Value.Year == year);
            }
            if (month != null)
            {
                filter = filter.And(m => m.PharmacyId == pharmacyId && m.CreatedAt!.Value.Month == month);
            }

            var result = await _unitOfWork.Pharmacies.GetEnhancedAsync(
                filter: p => p.Id == pharmacyId,
                selector: p => new PharmacyDetailsDto
                {
                    Id = pharmacyId,
                    Name = p.Name,
                    Location = p.Location,
                    PhoneNumber = p.PhoneNumber,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    NumberOfPharmacist = p.pharmacists.Count(x => !x.User.IsDeleted),
                    TotlaProfit = p.MedicineSales.Where(filter.InvokeEFCore).Sum(x => x.TotalAmount),
                    pharmacists = p.pharmacists.Select(x => new PharmacistDto
                    {
                        FullName = x.User.FirstName + " " + x.User.LastName,
                        Email = x.User.Email!,
                        PhoneNumber = x.User.PhoneNumber!,
                        PharmacistId = x.ApplicationUserId,
                        IsActive = x.User.IsActive,
                        HireDate = x.HireDate,
                        TotalSales = x.MedicineSales.Where(filter.InvokeEFCore).Sum(x => x.TotalAmount),
                        IsDeleted = x.User.IsDeleted

                    }).ToList()
                }
                );

            return result;
        }

        public async Task CreatePharmacy(NewPharmacyDto dto)
        {
            var newPharmacy = new Pharmacy
            {
                Name = dto.Name,
                Location = dto.Location,
                PhoneNumber = dto.PhoneNumber,
            };

            await _unitOfWork.Pharmacies.AddAsync(newPharmacy);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePharmacy(Guid pharmacyId)
        {
            var isExist = await _unitOfWork.Pharmacies.IsExistAsync(pharmacyId);
            if (!isExist) throw new Exception("Pharmacy Not Exist");

            var isHavePharmacists = await _unitOfWork.Pharmacists.FirstOrDefaultAsync(p => p.PharmacyId == pharmacyId) != null;
            if (!isHavePharmacists) throw new Exception("Pharmacy Has Pharmacists, Delete Them First");

            _unitOfWork.Pharmacies.Delete(pharmacyId);
            await _unitOfWork.CompleteAsync();
        }



        public async Task UpdatePharmacy(Guid pharmacyId, UpdatePharmacyDto dto)
        {
            var pharmacy = await _unitOfWork.Pharmacies.FirstOrDefaultAsync(p => p.Id == pharmacyId);
            if (pharmacy is null) throw new Exception("Pharmacy Not Exist");

            pharmacy.Name = dto.Name;
            pharmacy.Location = dto.Location;
            pharmacy.PhoneNumber = dto.PhoneNumber;

            await _unitOfWork.CompleteAsync();
        }
    }
}
