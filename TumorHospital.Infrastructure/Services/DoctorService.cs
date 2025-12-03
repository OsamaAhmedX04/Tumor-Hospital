using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DoctorDetailsDto> GetDoctorDetails(string doctorId)
        {
            var doctorDetails = await _unitOfWork.Doctors.GetEnhancedAsync(
                filter: d => d.ApplicationUserId == doctorId && d.User.IsActive,
                selector: d => new DoctorDetailsDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    ProfileImageUrl = d.ProfilePicturePath == null ?
                                    null : SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    Gender = d.Gender,
                    Bio = d.Bio,
                    Specialization = d.Specialization!.Name,
                    WorkingDays = d.Schedules.Select(s => new DoctorWorkDayDto
                    {
                        Day = s.DayOfWeek.ToString(),
                        FromTime = s.StartTime,
                        ToTime = s.EndTime
                    }).ToList()
                }
                ) ?? throw new Exception("DoctorNot Found");

            foreach (var day in doctorDetails.WorkingDays)
            {
                Day dayEnum = Enum.Parse<Day>(day.Day);
                day.IsAvailable = await IsAvailableDay(doctorId,dayEnum);
            }

            return doctorDetails;
        }

        private async Task<bool> IsAvailableDay(string doctorId, Day dayOfWeek)
        {
            var isHaveSurgery = await _unitOfWork.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.DayOfWeek == dayOfWeek &&
                               a.Reason == AppointmentReason.Surgery &&
                               a.Status == AppointmentStatus.Approved);

            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.DoctorId == doctorId &&
                             a.DayOfWeek == dayOfWeek &&
                             a.Status == AppointmentStatus.Approved &&
                             (a.Reason == AppointmentReason.Consultation || a.Reason == AppointmentReason.FollowUp),
                selector: a => new { a.Id}
                );
            var isFullyBooked = appointments.Count() == Appointments.NumberOfConsultationsOrFollowUpsPerDay;

            return !isHaveSurgery && !isFullyBooked;

        }
         

        public async Task<PageSourcePagination<DoctorDto>> GetDoctors(int pageSize, int pageNumber, string? workDay = null)
        {
            PageSourcePagination<DoctorDto> doctors;
            if (string.IsNullOrEmpty(workDay))
            {
                doctors = await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: d => d.User.IsActive,
                selector: d => new DoctorDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    ProfileImageUrl = d.ProfilePicturePath == null ?
                                    null : SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    Gender = d.Gender
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
            }
            else
            {
                var day = workDay!.ToLower() switch
                {
                    "monday" => Day.Monday,
                    "tuesday" => Day.Tuesday,
                    "wednesday" => Day.Wednesday,
                    "thursday" => Day.Thursday,
                    "friday" => Day.Friday,
                    "saturday" => Day.Saturday,
                    "sunday" => Day.Sunday,
                    _ => throw new ArgumentException("Invalid day of the week"),
                };
                doctors = await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: d => d.Schedules.Any(s => s.DayOfWeek == day) && d.User.IsActive,
                selector: d => new DoctorDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    ProfileImageUrl = d.ProfilePicturePath == null ?
                                    null : SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    Gender = d.Gender
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
            }

            return doctors;
        }
    }
}
