using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class SpecializationService : ISpecializationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        public SpecializationService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task AddSpecialization(SpecializationDto model)
        {
            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Specialization name cannot be empty.");
            if (model.Name.Length > 50)
                throw new ArgumentException("Specialization name Length Can not excced 50 char");

            bool isExisting = await _unitOfWork.Specializations
                .AnyAsync(s => s.Name.ToLower() == model.Name.ToLower());
            if (isExisting)
                throw new InvalidOperationException("Specialization with the same name already exists.");


            var specialization = new Specialization
            {
                Name = model.Name,
                Description = string.IsNullOrEmpty(model.Description) ? "N/A" : model.Description
            };
            await _unitOfWork.Specializations.AddAsync(specialization);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("SpecializationNames");
        }


        public async Task UpdateSpecialization(Guid id, SpecializationDto model)
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(id);
            if (specialization == null)
                throw new KeyNotFoundException("Specialization not found.");

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Specialization name cannot be empty.");
            if (model.Name.Length > 50)
                throw new ArgumentException("Specialization name Length Can not excced 50 char");

            bool isExisting = await _unitOfWork.Specializations
                .AnyAsync(s => s.Name.ToLower() == model.Name.ToLower());
            if (isExisting)
                throw new InvalidOperationException("Specialization with the same name already exists.");

            specialization.Name = model.Name;
            specialization.Description = string.IsNullOrEmpty(model.Description) ? "N/A" : model.Description;
            await _unitOfWork.CompleteAsync();
            _cache.Remove("SpecializationNames");
        }

        public async Task DeleteSpecialization(Guid id)
        {
            bool isExisting = await _unitOfWork.Specializations.IsExistAsync(id);
            if (!isExisting)
                throw new KeyNotFoundException("Specialization not found.");
            _unitOfWork.Specializations.Delete(id);
            await _unitOfWork.CompleteAsync();
            _cache.Remove("SpecializationNames");
        }

        public async Task AssignSpecializationToDoctor(string doctorId, string specializationName)
        {
            var doctor = await _unitOfWork.Doctors.GetAsync(
                filter: d => d.ApplicationUserId == doctorId,
                selector: d => d
                );

            if (doctor is null)
                throw new Exception("Doctor not exist");

            var specialization = await _unitOfWork.Specializations.GetAsync(
                filter: s => s.Name == specializationName,
                selector: s => new
                {
                    Id = s.Id,
                    Name = s.Name
                }
                );

            if (specialization == null || specialization.Name != specializationName)
                throw new BadHttpRequestException("This Specialization not exist");

            doctor.SpecializationId = specialization.Id;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<SpecializationDetailsDto>> GetSpecializations()
            => await _unitOfWork.Specializations.GetAllAsync(
                selector: s => new SpecializationDetailsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description ?? "N/A",
                    CreatedAt = s.CreatedAt
                }
                );

        public async Task<List<string>> GetSpecializationNames()
        {
            if (_cache.TryGetValue("SpecializationNames", out List<string> names))
            {
                return names;
            }
            else
            {
                names = await _unitOfWork.Specializations.GetAllAsync(
                    selector: s => s.Name
                    );
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
                };
                _cache.Set("SpecializationNames", cacheOptions);

                return names;
            }

        }


    }
}
