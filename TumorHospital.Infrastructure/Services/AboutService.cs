using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Request.About_Contact;
using TumorHospital.Application.DTOs.Response.About_Contact;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class AboutService : IAboutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public AboutService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<AboutResponse> GetAboutAsync()
        {
            var cacheKey = "about";

            if (_cache.TryGetValue(cacheKey, out AboutResponse cached))
                return cached;

            var about = (await _unitOfWork.AboutInfos.GetAllAsync(a => a))
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefault();

            if (about == null)
                return null;

            var response = new AboutResponse
                {
                    HospitalName = about.HospitalName,
                    Description = about.Description,
                    Mission = about.Mission,
                    Vision = about.Vision,
                    Email = about.Email,
                    Phone = about.Phone,
                    TotalDoctors = await _unitOfWork.Doctors.Count(),
                    TotalPatients = await _unitOfWork.Patients.Count(),
                    TotalReceptionist = await _unitOfWork.Receptionists.Count()
            };

            _cache.Set(
                cacheKey,
                response,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                }
            );

            return response;
        }
        public async Task AddOrUpdateAsync(AddAboutInfoDto dto)
        {
            var existing = (await _unitOfWork.AboutInfos.GetAllAsync(a => a))
                           .FirstOrDefault();

            if (existing == null)
            {
                existing = new AboutInfo();
                await _unitOfWork.AboutInfos.AddAsync(existing);
            }

            existing.HospitalName = dto.HospitalName;
            existing.Description = dto.Description;
            existing.Mission = dto.Mission;
            existing.Vision = dto.Vision;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;

            await _unitOfWork.CompleteAsync();

            _cache.Remove("about");
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.AboutInfos.GetByIdAsync(id);
            if (entity == null) return;

            _unitOfWork.AboutInfos.Delete(id);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("about");
        }

    }

}
