using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Helpers;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

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
            IScheduleService scheduleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _scheduleService = scheduleService;
        }



        public async Task AppointConsultation(NewConsultationAppointmentDto appointment)
        {
            if (IsInValidTimeToAppoint())
                throw new ApplicationException("Appointments Is Closed Between 12 AM and 5 AM");
            var isUsersExist = await _userManager.FindByIdAsync(appointment.PatientId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }
        public async Task AppointFollowUp(NewFollowUpAppointmentDto appointment)
        {
            if (IsInValidTimeToAppoint())
                throw new ApplicationException("Appointments Is Closed Between 12 AM and 5 AM");
            var isUsersExist = await _userManager.FindByIdAsync(appointment.PatientId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }
        public async Task AppointSurgery(NewSurgeryAppointmentDto appointment)
        {
            if (IsInValidTimeToAppoint())
                throw new ApplicationException("Appointments Is Closed Between 12 AM and 5 AM");
            var isUsersExist = await _userManager.FindByIdAsync(appointment.PatientId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }



        public async Task<PageSourcePagination<AppointmentDto>> GetAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null)
        {
            Expression<Func<Appointment, bool>>? filter = null;

            AppointmentReason? reason = null;
            AppointmentStatus? status = null;

            if (!string.IsNullOrEmpty(appointmentReason))
            {
                if (!Enum.TryParse<AppointmentReason>(appointmentReason, out AppointmentReason outReason))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    reason = outReason;
                    filter = a => a.Reason == reason;
                }
            }

            if (!string.IsNullOrEmpty(appointmentStatus))
            {
                if (!Enum.TryParse<AppointmentStatus>(appointmentStatus, out AppointmentStatus outStatus))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    status = outStatus;
                    filter = status != null && reason != null
                           ? a => a.Reason == reason && a.Status == status
                           : a => a.Status == status;
                }

            }


            var appointments = await _unitOfWork.Appointments.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: a => new AppointmentDto
                {
                    AppointmentId = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    DoctorName = a.Doctor!.User.FirstName + " " + a.Doctor.User.LastName,
                    DoctorImagePath = SupabaseConstants.PrefixSupaURL + a.Doctor.ProfilePicturePath,
                    Reason = a.Reason.ToString(),
                    DayOfWeek = a.DayOfWeek.ToString(),
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    Status = a.Status.ToString(),
                    IsPrescriptionExist = a.Prescription != null,
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                pageSize: 15,
                pageNumber: pageNumber
                );

            return appointments;
        }

        public async Task<PageSourcePagination<AppointmentDto>> GetPatientAppointments(int pageNumber, string patientId, string? appointmentReason = null, string? appointmentStatus = null)
        {
            Expression<Func<Appointment, bool>>? filter = a => a.PatientId == patientId;

            AppointmentReason? reason = null;
            AppointmentStatus? status = null;

            if (!string.IsNullOrEmpty(appointmentReason))
            {
                if (!Enum.TryParse<AppointmentReason>(appointmentReason, out AppointmentReason outReason))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    reason = outReason;
                    filter = a => a.Reason == reason;
                }
            }

            if (!string.IsNullOrEmpty(appointmentStatus))
            {
                if (!Enum.TryParse<AppointmentStatus>(appointmentStatus, out AppointmentStatus outStatus))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    status = outStatus;
                    filter = status != null && reason != null
                           ? a => a.Reason == reason && a.Status == status
                           : a => a.Status == status;
                }

            }


            var appointments = await _unitOfWork.Appointments.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: a => new AppointmentDto
                {
                    AppointmentId = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    DoctorName = a.Doctor!.User.FirstName + " " + a.Doctor.User.LastName,
                    DoctorImagePath = SupabaseConstants.PrefixSupaURL + a.Doctor.ProfilePicturePath,
                    Reason = a.Reason.ToString(),
                    DayOfWeek = a.DayOfWeek.ToString(),
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    Status = a.Status.ToString(),
                    IsPrescriptionExist = a.Prescription != null,
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                pageSize: 15,
                pageNumber: pageNumber
                );

            return appointments;
        }

        public async Task<PageSourcePagination<AppointmentDto>> GetDoctorAppointments(int pageNumber, string doctorId, string? appointmentReason = null, string? appointmentStatus = null)
        {
            Expression<Func<Appointment, bool>>? filter = a => a.DoctorId == doctorId;

            AppointmentReason? reason = null;
            AppointmentStatus? status = null;

            if (!string.IsNullOrEmpty(appointmentReason))
            {
                if (!Enum.TryParse<AppointmentReason>(appointmentReason, out AppointmentReason outReason))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    reason = outReason;
                    filter = a => a.Reason == reason;
                }
            }

            if (!string.IsNullOrEmpty(appointmentStatus))
            {
                if (!Enum.TryParse<AppointmentStatus>(appointmentStatus, out AppointmentStatus outStatus))
                    throw new Exception("Invalid Appointment Reason");
                else
                {
                    status = outStatus;
                    filter = status != null && reason != null
                           ? a => a.Reason == reason && a.Status == status
                           : a => a.Status == status;
                }

            }


            var appointments = await _unitOfWork.Appointments.GetAllPaginatedEnhancedAsync(
                filter: filter,
                selector: a => new AppointmentDto
                {
                    AppointmentId = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    DoctorName = a.Doctor!.User.FirstName + " " + a.Doctor.User.LastName,
                    DoctorImagePath = SupabaseConstants.PrefixSupaURL + a.Doctor.ProfilePicturePath,
                    Reason = a.Reason.ToString(),
                    DayOfWeek = a.DayOfWeek.ToString(),
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    Status = a.Status.ToString(),
                    IsPrescriptionExist = a.Prescription != null,
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                pageSize: 15,
                pageNumber: pageNumber
                );

            return appointments;
        }

        public List<string> AppointmentReasons()
        {
            var reasons = Enum.GetNames(typeof(AppointmentReason)).ToList();
            return reasons;
        }

        public async Task AcceptAppointment(Guid appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new ArgumentException("Appointment not found.");

            appointment.Status = AppointmentStatus.Approved;

            var doctorDaySchedule = await _unitOfWork.DoctorSchedules.GetEnhancedAsync(
                filter: ds => ds.DoctorId == appointment.DoctorId && ds.DayOfWeek == appointment.DayOfWeek,
                selector: ds => new
                {
                    ds.StartTime,
                    ds.EndTime
                });

            var lastAppointmentInRequestedDay = await _unitOfWork.Appointments
            .GetAllAsIQueryable()
            .AsNoTracking()
            .Where(a => a.Status == AppointmentStatus.Approved && a.DayOfWeek == appointment.DayOfWeek)
            .OrderBy(a => a.FromTime) // order by start time
            .LastOrDefaultAsync();

            if (lastAppointmentInRequestedDay is null)
            {
                appointment.FromTime = doctorDaySchedule!.StartTime;
                appointment.ToTime = doctorDaySchedule.StartTime.Add(TimeSpan.FromMinutes(30));
            }
            else
            {
                appointment.FromTime = lastAppointmentInRequestedDay.ToTime;
                appointment.ToTime = lastAppointmentInRequestedDay.ToTime!.Value.Add(TimeSpan.FromMinutes(30));

                if (appointment.ToTime == doctorDaySchedule!.EndTime)
                    await RejectRestOfAppointments(appointment.DoctorId!,appointment.DayOfWeek);
            }

            appointment.AttendenceDate = DayHelper.GetDateThisWeek(appointment.DayOfWeek);

            var appointmentCost = await _unitOfWork.Doctors.GetEnhancedAsync(
                filter: d => d.ApplicationUserId == appointment.DoctorId,
                selector: d => new
                {
                    d.ConsultationCost,
                    d.FollowUpCost,
                    d.SurgeryCost
                });

            var amount = appointment.Reason switch
            {
                AppointmentReason.Consultation => appointmentCost!.ConsultationCost,
                AppointmentReason.FollowUp => appointmentCost!.FollowUpCost,
                AppointmentReason.Surgery => appointmentCost!.SurgeryCost,
                _ => 0.00m
            };
            var bill = new Bill
            {
                PatientId = appointment.PatientId,
                AppointmentId = appointment.Id,
                TotalAmount = amount!.Value,
                Code = Generator.GenerateRandomBillCode(),
                Status = BillStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Bills.AddAsync(bill);

            await _unitOfWork.CompleteAsync();

            var appointmentInfo = await _unitOfWork.Appointments.GetEnhancedAsync(
                filter: a => a.Id == appointment.Id,
                selector: a => new
                {
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    PatientEmail = a.Patient.User.Email,
                    HospitalName = a.Doctor!.Hospital!.Name,
                    Location = a.Doctor.Hospital.Government + " - " + a.Doctor.Hospital.Address
                }
                );

            BackgroundJob.Enqueue<IEmailService>(
                service => service.SendEmailAsync(
                    appointmentInfo!.PatientEmail!,
                    "Appointment has been Accepted",
                    EmailBody.GetAppointmentAcceptedEmailBody(
                        appointmentInfo.PatientName,
                        appointmentInfo.HospitalName,
                        appointmentInfo.Location,
                        appointment.AttendenceDate.ToString()!,
                        appointment.FromTime.ToString()!
                        )
                ));


            var dateTimeOfAppointment = appointment.AttendenceDate.Value
                            .ToDateTime(new TimeOnly(0, 0))
                            .Add(appointment.FromTime!.Value);
            var hours = DateTime.Now.Subtract(dateTimeOfAppointment).Hours;
            if (hours > 24)
            {
                BackgroundJob.Schedule<IEmailService>(
                service => service.SendEmailAsync(
                    appointmentInfo!.PatientEmail!,
                    "Appointment has been Accepted",
                    EmailBody.GetAppointmentReminderEmailBody(
                        appointmentInfo.PatientName,
                        appointmentInfo.HospitalName,
                        appointmentInfo.Location,
                        appointment.AttendenceDate.ToString()!,
                        appointment.FromTime.ToString()!
                        )
                ),
                TimeSpan.FromHours(24));
            }


        }
        public async Task RejectAppointment(Guid appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new ArgumentException("Appointment not found.");
            appointment.Status = AppointmentStatus.Rejected;
            await _unitOfWork.CompleteAsync();
        }

        public async Task AttendPatientToAppointment(string patientId, Guid appointmentId)
        {
            if (await _unitOfWork.Patients.IsExistAsync(patientId))
                throw new Exception("Patient with this id does not exist");
            if (await _unitOfWork.Appointments.IsExistAsync(appointmentId))
                throw new Exception("Appointment with this id does not exist");

            await _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .Where(a => a.Status == AppointmentStatus.Approved && a.Id == appointmentId && a.PatientId == patientId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(a => a.Status, AppointmentStatus.Completed));
        }

        private async Task RejectRestOfAppointments(string doctorId, Day day)
        {
            await _unitOfWork.Appointments.GetAllAsIQueryable()
                .Where(a => a.Status == AppointmentStatus.Pending && a.DoctorId == doctorId && a.DayOfWeek == day)
                .ExecuteUpdateAsync(setter => setter.SetProperty(a => a.Status, AppointmentStatus.Rejected));
        }

        private bool IsInValidTimeToAppoint()
            => DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 5;


    }
}
