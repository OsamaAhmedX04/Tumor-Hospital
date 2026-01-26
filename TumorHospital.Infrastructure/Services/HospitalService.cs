using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Request.Hospital;
using TumorHospital.Application.DTOs.Response.Hospital;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public HospitalService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task AddHospital(HospitalDto model)
        {
            if (await IsDuplicatedAddress(model.Address))
                throw new Exception("This Address Already Exist");
            if (await IsDuplicatedName(model.Name))
                throw new Exception("This Name Already Exist");

            var hospital = _mapper.Map<Hospital>(model);
            await _unitOfWork.Hospitals.AddAsync(hospital);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("HospitalsNames");
            _cache.Remove("HospitalsGovernments");
        }

        public async Task DeleteHospital(Guid id)
        {
            var isExist = await _unitOfWork.Hospitals.IsExistAsync(id);
            if (!isExist)
                throw new Exception("Id Not Exist");

            var isThereAnyEmployees =
                await _unitOfWork.Doctors.AnyAsync(d => d.HospitalId == id)
                ||
                await _unitOfWork.Receptionists.AnyAsync(r => r.HospitalId == id)
                ;

            if (isThereAnyEmployees)
                throw new Exception("There Is Already Doctors Or Receptionists in this Hospital");

            _unitOfWork.Hospitals.Delete(id);
            await _unitOfWork.CompleteAsync();
            _cache.Remove("HospitalsNames");
            _cache.Remove("HospitalsGovernments");
        }

        public async Task UpdateHospital(Guid id, HospitalDto model)
        {
            var hospital = await _unitOfWork.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
            if (hospital is null)
                throw new Exception("Id Not Exist");
            if (!await IsDuplicatedAddress(model.Address))
                throw new Exception("This Address Already Exist");
            if (await IsDuplicatedName(model.Name))
                throw new Exception("This Name Already Exist");
            var numberOfCurrentDoctors = await _unitOfWork.Doctors.Count(d => d.HospitalId == id);
            var numberOfCurrentReceptionist = await _unitOfWork.Receptionists.Count(r => r.HospitalId == id);

            if (model.MaxNumberOfDoctors < numberOfCurrentDoctors)
                throw new Exception($"There is {numberOfCurrentDoctors} currently so you can go with only {model.MaxNumberOfDoctors}");
            if (model.MaxNumberOfReceptionists < numberOfCurrentReceptionist)
                throw new Exception($"There is {numberOfCurrentReceptionist} currently so you can go with only {model.MaxNumberOfReceptionists}");

            hospital.Name = model.Name;
            hospital.Government = model.Government;
            hospital.Address = model.Address;
            hospital.MaxNumberOfDoctors = model.MaxNumberOfDoctors;
            hospital.MaxNumberOfReceptionists = model.MaxNumberOfReceptionists;

            await _unitOfWork.CompleteAsync();
            _cache.Remove("HospitalsNames");
            _cache.Remove("HospitalsGovernments");
        }

        public async Task<PageSourcePagination<DoctorDto>> GetHospitalDoctors(Guid id, string? doctorName = null, int pageNumber = 1)
        {
            return await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: d => d.HospitalId == id && (d.User.FirstName + " " + d.User.LastName).Contains(doctorName),
                selector: d => new DoctorDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    Gender = d.Gender,
                    ProfileImageUrl = SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    IsActive = d.User.IsActive
                },
                pageNumber: pageNumber,
                pageSize: 10
                );
        }
        public async Task<DoctorInformationDto> GetHospitalDoctor(string doctorId)
        {
            var doctorDetails = await _unitOfWork.Doctors.GetEnhancedAsync(
                filter: d => d.ApplicationUserId == doctorId && d.User.IsActive,
                selector: d => new DoctorInformationDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    ProfileImageUrl = d.ProfilePicturePath == null ?
                                    null : SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    Gender = d.Gender,
                    Bio = d.Bio,
                    Specialization = d.Specialization!.Name,
                    IsSurgeon = d.IsSurgeon,
                    ConsultationCost = d.ConsultationCost,
                    FollowUpCost = d.FollowUpCost,
                    SurgeryCost = !d.IsSurgeon ? null : d.SurgeryCost,
                    WorkingDays = d.Schedules.Select(s => new DoctorWorkDayPreifDto
                    {
                        Day = s.DayOfWeek.ToString(),
                        FromTime = s.StartTime,
                        ToTime = s.EndTime
                    }).ToList()
                }
                ) ?? throw new Exception("Doctor Not Found");

            return doctorDetails;
        }

        public async Task<PageSourcePagination<ReceptionistDto>> GetHospitalReceptionists(Guid id, string? receptionistName = null, int pageNumber = 1)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: r => r.HospitalId == id && (r.User.FirstName + " " + r.User.LastName).Contains(receptionistName),
                selector: r => new ReceptionistDto
                {
                    Id = r.ApplicationUserId,
                    Name = r.User.FirstName + " " + r.User.LastName,
                },
                pageNumber: pageNumber,
                pageSize: 10
                );
        }

        public async Task<List<HospitalInfoDto>> GetHospitals()
        {
            return await _unitOfWork.Hospitals.GetAllAsyncEnhanced(
                selector: h => new HospitalInfoDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Government = h.Government,
                    Address = h.Address,
                },
                orderBy: h => h.OrderBy(h => h.Name)
                );
        }

        public async Task<HospitalDashboardDto> GetHospitalDashboard(Guid id)
        {
            return await _unitOfWork.Hospitals.GetEnhancedAsync(
                filter: h => h.Id == id,
                selector: h => new HospitalDashboardDto
                {
                    MaxNumberOfDoctors = h.MaxNumberOfDoctors,
                    MaxNumberOfReceptionists = h.MaxNumberOfReceptionists,
                    NumberOfDoctors = h.Doctors.Count(),
                    NumberOfReceptionists = h.Receptionists.Count(),
                }
                ) ?? throw new Exception("Hospital Not Exist");
        }

        public async Task<List<string>> GetHospitalsNames()
        {
            if (!_cache.TryGetValue("HospitalsNames", out List<string>? names))
            {
                names = await _unitOfWork.Hospitals.GetAllAsync(selector: h => h.Name);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
                };
                _cache.Set("HospitalsNames", names);
            }
            return names ?? new List<string>();

        }

        public async Task<List<string>> GetHospitalGovernments()
        {
            if (!_cache.TryGetValue("HospitalsGovernments", out List<string>? governments))
            {
                governments = await _unitOfWork.Hospitals.GetAllAsync(selector: h => h.Government);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
                };
                _cache.Set("HospitalsGovernments", governments);
            }
            return governments ?? new List<string>();

        }

        private async Task<bool> IsDuplicatedAddress(string address)
            => await _unitOfWork.Hospitals.AnyAsync(h => h.Address == address);
        private async Task<bool> IsDuplicatedName(string name)
            => await _unitOfWork.Hospitals.AnyAsync(h => h.Name == name);


    }
}
