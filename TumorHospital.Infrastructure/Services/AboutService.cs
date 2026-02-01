using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Response.About_Contact;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

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
            var cacheKey = "about_en";

            if (_cache.TryGetValue(cacheKey, out AboutResponse cached))
                return cached;

            var response = new AboutResponse
                {
                    HospitalName = "Tumor Hospital",
                    Description = "Providing advanced cancer care with compassion",
                    Mission = "High-quality integrated medical care",
                    Vision = "To be a leader in cancer treatment",
                    Email = "tumorhospital@gmail.com",
                    Phone = "+20 113 456 7890",
                    Address = "Cairo, Egypt",
                    TotalDoctors = await _unitOfWork.Doctors.Count(),
                    TotalPatients = await _unitOfWork.Patients.Count(),
                    TotalReceptionist = await _unitOfWork.Receptionists.Count()
            };

            _cache.Set(
                cacheKey,
                response,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                }
            );

            return response;
        }
    }

}
