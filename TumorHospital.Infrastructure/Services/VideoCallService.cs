using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class VideoCallService : IVideoCallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VideoCallService> _logger;
        private readonly IHubContext<VideoCallHub> _hubContext;

        public VideoCallService(IUnitOfWork unitOfWork, ILogger<VideoCallService> logger, IHubContext<VideoCallHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<Guid> StartVideoCallAsync(string patientId, string doctorId, Guid appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} Appointment not found",
                    appointmentId,
                    patientId
                );
                throw new NotFoundException("Appointment not found");
            }

            if (appointment.PatientId != patientId ||
                appointment.DoctorId != doctorId)
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} Unauthorized video call",
                    appointmentId,
                    patientId
                );
                throw new UnauthorizedException("Unauthorized video call");
            }

            if (appointment.StartDateTime == null)
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} Appointment date is not scheduled",
                    appointmentId,
                    patientId
                );
                throw new Exception("Appointment date is not scheduled");
            }


            if (appointment.StartDateTime > DateTime.Now.AddMinutes(5))
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} Video call cannot start yet",
                    appointmentId,
                    patientId
                );
                throw new Exception("Video call cannot start yet");
            }

            if (appointment.StartDateTime < DateTime.Now.AddMinutes(-10))
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} Appointment expired",
                    appointmentId,
                    patientId
                );
                throw new Exception("Appointment expired");
            }

            if (appointment.Bill == null)
            {
                _logger.LogWarning(
                    "Video call blocked | AppointmentId: {AppointmentId} | Reason: No bill found",
                    appointmentId
                );
                throw new NotFoundException("Appointment bill has not been created yet");
            }

            if (appointment.Bill.Status != BillStatus.Paid)
            {
                _logger.LogWarning(
                    "Video call blocked | AppointmentId: {AppointmentId} | BillStatus: {Status}",
                    appointmentId,
                    appointment.Bill.Status
                );
                throw new Exception("Appointment bill must be paid before starting the video call");
            }

            var hasActiveCall = await _unitOfWork.VideoCalls
                .AnyAsync(vc =>
                    vc.AppointmentId == appointment.Id &&
                    vc.IsActive);

            if (hasActiveCall)
            {
                _logger.LogWarning(
                    "Invalid video call attempt | AppointmentId: {AppointmentId} | User: {UserId} An active video call already exists for this appointment",
                    appointmentId,
                    patientId
                );
                throw new Exception("An active video call already exists for this appointment");
            }

            var call = new VideoCall
            {
                CallerId = patientId,
                ReceiverId = doctorId,
                AppointmentId = appointmentId,
                StartedAt = DateTime.Now,
                IsActive = true,
                Status = CallStatus.Ringing
            };

            await _unitOfWork.VideoCalls.AddAsync(call);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Video call Ringing | CallId: {CallId} | AppointmentId: {AppointmentId} | Caller: {Caller} | Receiver: {Receiver}",
                call.Id,
                appointmentId,
                patientId,
                doctorId
            );

            BackgroundJob.Schedule<IVideoCallService>(
                s => s.EndCallAsync(call.Id, null, "Timeout"),
                TimeSpan.FromMinutes(30)
            );

            return call.Id;
        }

        public async Task AcceptCallAsync(Guid callId, string userId)
        {
            var call = await _unitOfWork.VideoCalls.GetByIdAsync(callId);
            if (call == null)
                throw new NotFoundException("Call not found");

            if (!call.IsActive || call.Status != CallStatus.Ringing)
                throw new Exception("Call is not in a ringable state");

            if (call.ReceiverId != userId)
                throw new UnauthorizedException("Only the doctor can accept the call");

            call.Status = CallStatus.Accepted;

            _unitOfWork.VideoCalls.Update(call);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Call accepted | CallId: {CallId} | Doctor: {DoctorId}",
                callId,
                userId
            );

            await _hubContext.Clients.User(call.CallerId).SendAsync("CallAccepted", call.Id);
        }

        public async Task RejectCallAsync(Guid callId, string userId)
        {
            var call = await _unitOfWork.VideoCalls.GetByIdAsync(callId);
            if (call == null)
                throw new NotFoundException("Call not found");

            if (!call.IsActive || call.Status != CallStatus.Ringing)
                throw new Exception("Call cannot be rejected");

            if (call.ReceiverId != userId)
                throw new UnauthorizedException("Only the doctor can reject the call");

            call.IsActive = false;
            call.Status = CallStatus.Rejected;
            call.EndedAt = DateTime.Now;

            _unitOfWork.VideoCalls.Update(call);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Video call has been Rejected | CallId: {CallId}",
                call.Id
            );

            await _hubContext.Clients.User(call.CallerId).SendAsync("CallRejected", call.Id);
        }

        public async Task EndCallAsync(Guid callId, string userId, string reason)
        {
            var call = await _unitOfWork.VideoCalls.GetByIdAsync(callId);
            if (call == null || !call.IsActive) return;

            if (userId != null &&
                call.CallerId != userId &&
                call.ReceiverId != userId)
                throw new UnauthorizedException("You are not part of this call");

            call.IsActive = false;
            call.EndedAt = DateTime.Now;

            call.Status = reason switch
            {
                "Timeout" => CallStatus.Timeout,
                _ => CallStatus.Ended
            };

            _unitOfWork.VideoCalls.Update(call);
            await _unitOfWork.CompleteAsync();

            var duration = call.EndedAt.HasValue
            ? (call.EndedAt.Value - call.StartedAt).TotalMinutes
            : 0;

            _logger.LogInformation(
                "Video call ended | CallId: {CallId} | DurationMinutes: {Duration} | Reason: {Reason}",
                call.Id,
                duration,
                reason
            );

            await _hubContext.Clients.Group(callId.ToString()).SendAsync("CallEnded", call.Id);
        }
    }
}
