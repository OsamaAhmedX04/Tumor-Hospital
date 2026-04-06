using LinqKit;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Helpers;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
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

        public async Task<DoctorDetailsDto> GetDoctorDetails(string doctorId, string patientId)
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
                    IsVideoCallDoctor = d.IsVideoCallDoctor,
                    ConsultationCost = d.ConsultationCost,
                    FollowUpCost = d.FollowUpCost,
                    VideoCallCost = !d.IsVideoCallDoctor ? null : d.VideoCallCost,
                    Location = d.Hospital!.Address + " - " + d.Hospital.Government,
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
                day.IsAvailable = await IsAvailableDay(doctorId, dayEnum) && !DayHelper.IsDayInPast(dayEnum);
                day.Date = DayHelper.GetDateThisWeek(dayEnum);
            }

            var isAbleToAppointConsultation = !await _unitOfWork.Appointments.AnyAsync(
                filter: a =>
                a.DoctorId == doctorId && a.PatientId == patientId
                && (a.Status == AppointmentStatus.Approved || a.Status == AppointmentStatus.Pending));

            var isAbleToAppointFollowUp = await _unitOfWork.Appointments.AnyAsync(
                filter: a =>
                a.DoctorId == doctorId && a.PatientId == patientId &&
                a.Reason == AppointmentReason.Consultation && a.Status == AppointmentStatus.Completed);

            var isAbleToAppointVideoCall = !await _unitOfWork.Appointments.AnyAsync(
                filter: a =>
                a.DoctorId == doctorId && a.PatientId == patientId
                && (a.Status == AppointmentStatus.Approved || a.Status == AppointmentStatus.Pending));

            doctorDetails.IsAbleToAppointConsultation = isAbleToAppointConsultation;
            doctorDetails.IsAbleToAppointFollowUp = isAbleToAppointFollowUp;
            doctorDetails.IsAbleToAppointVideoCall = doctorDetails.IsVideoCallDoctor && isAbleToAppointVideoCall;


            return doctorDetails;
        }

        private async Task<bool> IsAvailableDay(string doctorId, Day dayOfWeek)
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.DoctorId == doctorId &&
                             a.DayOfWeek == dayOfWeek &&
                             a.Status == AppointmentStatus.Approved &&
                             (a.Reason == AppointmentReason.Consultation || a.Reason == AppointmentReason.FollowUp || a.Reason == AppointmentReason.VideoCall),
                selector: a => new { a.Id }
                );
            var isFullyBooked = appointments.Count() == Appointments.NumberOfAppointmentsPerDay;

            return !isFullyBooked;
        }




        public async Task<PageSourcePagination<DoctorDto>> GetDoctors(
    int pageNumber, string? workDay = null, bool? IsVideoCallDoctor = null, string? government = null, string? specializationName = null)
        {
            Expression<Func<Doctor, bool>> filter = d => d.User.IsActive;

            if (!string.IsNullOrEmpty(workDay))
            {
                var day = workDay.ToLower() switch
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
                filter = filter.And(d => d.Schedules.Any(s => s.DayOfWeek == day));
            }

            if (IsVideoCallDoctor.HasValue)
                filter = filter.And(d => d.IsVideoCallDoctor == IsVideoCallDoctor.Value);

            if (!string.IsNullOrEmpty(government))
                filter = filter.And(d => d.Hospital != null && d.Hospital.Government == government);

            if (!string.IsNullOrEmpty(specializationName))
                filter = filter.And(d => d.Specialization != null && d.Specialization.Name == specializationName);

            var doctors = await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: d => new DoctorDto
                {
                    Id = d.ApplicationUserId,
                    FullName = d.User.FirstName + " " + d.User.LastName,
                    ProfileImageUrl = d.ProfilePicturePath == null
                        ? null
                        : SupabaseConstants.PrefixSupaURL + d.ProfilePicturePath,
                    Gender = d.Gender,
                    IsActive = true
                },
                pageSize: 15,
                pageNumber: pageNumber
            );

            return doctors;
        }
    }
}
