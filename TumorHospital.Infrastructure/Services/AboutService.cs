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

            // throw exption

            var response = new AboutResponse
            {
                Id = about.Id,
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
        public async Task AddAsync(AddAboutInfoDto dto)
        {
            var isExist = await _unitOfWork.AboutInfos.AnyAsync();

            if (isExist)
                throw new Exception("About Information Already Exist You Can Update It");

            var about = new AboutInfo
            {
                HospitalName = dto.HospitalName,
                Description = dto.Description,
                Mission = dto.Mission,
                Vision = dto.Vision,
                Email = dto.Email,
                Phone = dto.Phone
            };

            await _unitOfWork.AboutInfos.AddAsync(about);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("about");
        }

        public async Task UpdateAsync(Guid id, UpdateAboutInfoDto dto)
        {
            var about = await _unitOfWork.AboutInfos.FirstOrDefaultAsync(a => a.Id == id);

            if (about == null)
                throw new Exception("About Information Is Not Created Yet");

            about.HospitalName = dto.HospitalName;
            about.Description = dto.Description;
            about.Mission = dto.Mission;
            about.Vision = dto.Vision;
            about.Email = dto.Email;
            about.Phone = dto.Phone;

            await _unitOfWork.CompleteAsync();

            _cache.Remove("about");
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.AboutInfos.IsExistAsync(id);
            if (!entity) throw new Exception("About Not Exist");

            // throw exption

            _unitOfWork.AboutInfos.Delete(id);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("about");
        }

    }

}
