using AutoMapper;
using Hangfire;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IMeetingService _meetingService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IScheduleService _scheduleService;
        private readonly IOfferService _offerService;
        private readonly ILogger<AppointmentService> _logger;
        private readonly ICurrentUserService _currentUserService;
        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IScheduleService scheduleService,
            IOfferService offerService, ILogger<AppointmentService> logger, IMeetingService meetingService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _scheduleService = scheduleService;
            _offerService = offerService;
            _logger = logger;
            _meetingService = meetingService;
            _currentUserService = currentUserService;
        }

        public async Task AppointConsultation(NewConsultationAppointmentDto appointment)
        {
            if (AppointmentTimeService.IsInValidTimeToAppoint())
                throw new ApplicationException("Appointments Is Closed Between 12 AM and 5 AM");
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User must be authenticated to make an appointment.");

            var isUsersExist = await _userManager.FindByIdAsync(userId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            appointmentEntity.PatientId = userId;
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }
        public async Task AppointFollowUp(NewFollowUpAppointmentDto appointment)
        {
            if (AppointmentTimeService.IsInValidTimeToAppoint())
                throw new ApplicationException("Appointments Is Closed Between 12 AM and 5 AM");

            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User must be authenticated to make an appointment.");

            var isUsersExist = await _userManager.FindByIdAsync(userId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            appointmentEntity.PatientId = userId;
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }
        public async Task AppointVideoCall(NewVideoCallAppointmentDto appointment)
        {
            if (AppointmentTimeService.IsInValidTimeToAppoint())
                throw new ApplicationException($"Appointments Is Closed Between 12 AM and 5 AM");
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User must be authenticated to make an appointment.");

            var isUsersExist = await _userManager.FindByIdAsync(userId) != null &&
                              await _userManager.FindByIdAsync(appointment.DoctorId) != null;
            if (!isUsersExist)
                throw new ArgumentException("Patient or Doctor does not exist.");
            if (!await _scheduleService.IsWorkIn(appointment.DoctorId, appointment.DayOfWeek))
                throw new ArgumentException("Doctor is not working on that day.");

            var appointmentEntity = _mapper.Map<Appointment>(appointment);
            appointmentEntity.PatientId = userId;
            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<PageSourcePagination<AppointmentBriefDto>> GetAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null, int? month = null, int? year = null)
        {
            Expression<Func<Appointment, bool>>? filter = a => true;

            if (month.HasValue)
                filter = filter.And(a => a.AttendenceDate.HasValue && a.AttendenceDate.Value.Month == month);

            if (year.HasValue)
                filter = filter.And(a => a.AttendenceDate.HasValue && a.AttendenceDate.Value.Year == year);

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
                    throw new Exception("Invalid Appointment Status");
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
                selector: a => new AppointmentBriefDto
                {
                    AppointmentId = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    DoctorName = a.Doctor!.User.FirstName + " " + a.Doctor.User.LastName,
                    Reason = a.Reason.ToString(),
                    DayOfWeek = a.DayOfWeek.ToString(),
                    FromTime = a.FromTime,
                    ToTime = a.ToTime,
                    Status = a.Status.ToString(),
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                orderBy: a => a.OrderByDescending(a => a.RequestCreatedAt),
                pageSize: 15,
                pageNumber: pageNumber
                );

            return appointments;
        }
        public async Task<PageSourcePagination<AppointmentDto>> GetPatientAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User must be authenticated to make an appointment.");

            Expression<Func<Appointment, bool>>? filter = a => a.PatientId == userId;

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
                    IsHaveRayFile = a.Diagnostic != null,
                    IsVideoCall = a.Reason == AppointmentReason.VideoCall,
                    IsVideoCallAvailableToJoin = a.Reason ==
                            AppointmentReason.VideoCall &&
                            a.Status == AppointmentStatus.Approved &&
                            a.AttendenceDate == DateOnly.FromDateTime(DateTime.Now) &&
                            a.FromTime <= DateTime.Now.TimeOfDay &&
                            a.ToTime >= DateTime.Now.TimeOfDay,
                    VideoCallLink = a.MeetingJoinLink,
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate
                },
                orderBy: a => a.OrderByDescending(a => a.AttendenceDate),
                pageSize: 15,
                pageNumber: pageNumber
                );

            return appointments;
        }
        public async Task<PageSourcePagination<AppointmentDto>> GetDoctorAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User must be authenticated to make an appointment.");

            Expression<Func<Appointment, bool>>? filter = a => a.DoctorId == userId;

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
                    IsHaveRayFile = a.Diagnostic != null,
                    IsVideoCall = a.Reason == AppointmentReason.VideoCall,
                    IsVideoCallAvailableToJoin = a.Reason ==
                            AppointmentReason.VideoCall &&
                            a.Status == AppointmentStatus.Approved &&
                            a.AttendenceDate == DateOnly.FromDateTime(DateTime.Now) &&
                            a.FromTime <= DateTime.Now.TimeOfDay &&
                            a.ToTime >= DateTime.Now.TimeOfDay,
                    VideoCallLink = a.MeetingStartLink,
                    RequestCreatedAt = a.RequestCreatedAt,
                    AttendenceDate = a.AttendenceDate

                },
                orderBy: a => a.OrderByDescending(a => a.AttendenceDate),
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
            #region get valid time slot for this appointment
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found.");


            var doctorDaySchedule = await _unitOfWork.DoctorSchedules.GetEnhancedAsync(
                filter: ds => ds.DoctorId == appointment.DoctorId && ds.DayOfWeek == appointment.DayOfWeek,
                selector: ds => new
                {
                    ds.StartTime,
                    ds.EndTime
                });

            var appointmentedTimesForDoctorInRequestedDay = await _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .AsNoTracking()
                .Where(a => a.Status == AppointmentStatus.Approved && a.DayOfWeek == appointment.DayOfWeek && a.DoctorId == appointment.DoctorId)
                .Select(a => a.FromTime!.Value)
                .OrderBy(a => a).ToListAsync();

            var appointmentedTimesForPatientInRequestedDay = await _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .AsNoTracking()
                .Where(a => a.Status == AppointmentStatus.Approved && a.DayOfWeek == appointment.DayOfWeek && a.PatientId == appointment.PatientId)
                .Select(a => a.FromTime!.Value)
                .OrderBy(a => a).ToListAsync();


            TimeSlot? validTimeSlot = AppointmentTimeService.SelectValidTimeSlot(
                new HashSet<TimeSpan>(appointmentedTimesForPatientInRequestedDay), new HashSet<TimeSpan>(appointmentedTimesForDoctorInRequestedDay),
                doctorDaySchedule!.StartTime, doctorDaySchedule.EndTime);

            if (validTimeSlot == null)
            {
                appointment.Status = AppointmentStatus.Rejected;
                try
                {
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error while accepting appointment {AppointmentId}", appointmentId);
                    throw new DbUpdateConcurrencyException("The appointment was modified by another process. Please try again.");
                }
                throw new Exception("No available time for this appointment");
            }


            appointment.Status = AppointmentStatus.Approved;
            appointment.FromTime = validTimeSlot.FromTime;
            appointment.ToTime = validTimeSlot.ToTime;

            if (appointment.ToTime == doctorDaySchedule!.EndTime)
                await RejectRestOfAppointments(appointment.DoctorId!, appointment.DayOfWeek);

            appointment.AttendenceDate = DayHelper.GetDateThisWeek(appointment.DayOfWeek);

            #endregion

            #region create bill for this appointment
            var appointmentCost = await _unitOfWork.Doctors.GetEnhancedAsync(
                filter: d => d.ApplicationUserId == appointment.DoctorId,
                selector: d => new
                {
                    d.ConsultationCost,
                    d.FollowUpCost,
                    d.VideoCallCost
                });

            var amount = appointment.Reason switch
            {
                AppointmentReason.Consultation => appointmentCost!.ConsultationCost,
                AppointmentReason.FollowUp => appointmentCost!.FollowUpCost,
                AppointmentReason.VideoCall => appointmentCost!.VideoCallCost,
                _ => 0.00m
            };

            var billCode =
                $"{appointment.AttendenceDate.Value:yy}{appointment.AttendenceDate.Value.Month:D2}{appointment.AttendenceDate.Value.Day:D2}{Generator.GenerateRandomBillCode()}";
            var bill = new Bill
            {
                PatientId = appointment.PatientId,
                AppointmentId = appointment.Id,
                OriginalAmount = amount!.Value,
                Code = billCode,
                Status = BillStatus.Pending,
                CreatedAt = DateTime.Now
            };

            var total = bill.OriginalAmount;

            var offers = await _offerService.GetAllOffersAsync();

            var bestOffer = offers
                .OrderByDescending(o => o.DiscountPercentage)
                .FirstOrDefault();

            decimal discount = 0;

            if (bestOffer != null)
            {
                discount = total * (bestOffer.DiscountPercentage / 100m);

                bill.AppliedOfferId = bestOffer.Id;
                bill.AppliedOfferPercentage = bestOffer.DiscountPercentage;

                _logger.LogInformation(
                    "Applied offer {OfferTitle} ({Discount}%) on Bill {BillId}",
                    bestOffer.Title,
                    bestOffer.DiscountPercentage,
                    bill.Id
                );
            }

            else
            {
                bill.AppliedOfferId = null;
                bill.AppliedOfferPercentage = 0;
            }

            bill.DiscountAmount = discount;
            bill.FinalAmount = total - discount;

            if (appointment.Reason == AppointmentReason.VideoCall)
            {
                DateOnly date = appointment.AttendenceDate.Value;
                TimeSpan time = appointment.FromTime.Value;
                var datetime = date.ToDateTime(TimeOnly.FromTimeSpan(time));

                var meetingInfo = await _meetingService.CreateMeeting(datetime);
                appointment.MeetingStartLink = meetingInfo.StartUrl;
                appointment.MeetingJoinLink = meetingInfo.JoinUrl;
            }

            await _unitOfWork.Bills.AddAsync(bill);

            #endregion

            #region save changes with concurrency handling
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while accepting appointment {AppointmentId}", appointmentId);
                throw new DbUpdateConcurrencyException("The appointment was modified by another process. Please try again.");
            }

            _logger.LogInformation(
                "Creating bill for PatientId {PatientId}, AppointmentId {AppointmentId}",
                bill.PatientId,
                bill.AppointmentId
            );

            #endregion

            #region send email to patient to inform him that his appointment has been accepted
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
            #endregion

        }

        public async Task RejectAppointment(Guid appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new ArgumentException("Appointment not found.");
            appointment.Status = AppointmentStatus.Rejected;
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while accepting appointment {AppointmentId}", appointmentId);
                throw new DbUpdateConcurrencyException("The appointment was modified by another process. Please try again.");
            }
        }

        public async Task AttendPatientToAppointment(string patientId, Guid appointmentId)
        {
            var isPatientExist = await _unitOfWork.Patients.IsExistAsync(patientId);
            var isAppointmentExist = await _unitOfWork.Appointments.IsExistAsync(appointmentId);

            if (!isPatientExist)
                throw new Exception("Patient with this id does not exist");

            if (!isAppointmentExist)
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
    }
}
