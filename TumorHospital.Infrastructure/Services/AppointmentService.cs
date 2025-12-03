using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IScheduleService _scheduleService;
        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IScheduleService scheduleService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _scheduleService = scheduleService;
        }
        public async Task AppointConsultation(NewConsultationAppointmentDto appointment)
        {
            var isUsersExist = await _userManager.FindByIdAsync(appointment.PatientId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if(!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }

        public Task AppointFollowUp(NewConsultationAppointmentDto appointment)
        {
            throw new NotImplementedException();
        }

        public Task AppointSurgery(NewConsultationAppointmentDto appointment)
        {
            throw new NotImplementedException();
        }

        public async Task<PageSourcePagination<AppointmentDto>> GetAppointments(int pageNumber)
            => await _unitOfWork.Appointments.GetAllPaginatedEnhancedAsync(
                selector: a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    DoctorName = a.Doctor.User.FirstName + " " + a.Doctor.User.LastName,
                    DoctorImagePath = SupabaseConstants.PrefixSupaURL + a.Doctor.ProfilePicturePath,
                    Reason = a.Reason.ToString(),
                    DayOfWeek = a.DayOfWeek.ToString(),
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    Status = a.Status.ToString(),
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                pageSize: 15,
                pageNumber: pageNumber
                );
    }
}
