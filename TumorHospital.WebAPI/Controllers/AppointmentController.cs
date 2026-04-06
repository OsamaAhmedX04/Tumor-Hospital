using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IScheduleService _scheduleService;
        private readonly IValidator<NewConsultationAppointmentDto> _newAppointmentConsultaionValidator;
        private readonly IValidator<NewFollowUpAppointmentDto> _newAppointmentFollowUpValidator;
        private readonly IValidator<NewVideoCallAppointmentDto> _newAppointmentSurgeryValidator;

        public AppointmentController(
            IAppointmentService appointmentService,
            IValidator<NewConsultationAppointmentDto> newAppointmentConsultaionValidator,
            IScheduleService scheduleService,
            IValidator<NewFollowUpAppointmentDto> newAppointmentFollowUpValidator,
            IValidator<NewVideoCallAppointmentDto> newAppointmentSurgeryValidator)
        {
            _appointmentService = appointmentService;
            _newAppointmentConsultaionValidator = newAppointmentConsultaionValidator;
            _scheduleService = scheduleService;
            _newAppointmentFollowUpValidator = newAppointmentFollowUpValidator;
            _newAppointmentSurgeryValidator = newAppointmentSurgeryValidator;
        }

        [HttpPost("consultaion")]
        public async Task<IActionResult> AppointConsultation(NewConsultationAppointmentDto appointmentDto)
        {
            var validationResult = await _newAppointmentConsultaionValidator.ValidateAsync(appointmentDto);
            if (validationResult.IsValid)
            {
                try
                {
                    await _appointmentService.AppointConsultation(appointmentDto);
                    return Ok(new { Message = "Appointment Created Successfully" });
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("Time", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }

            }
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPost("followup")]
        public async Task<IActionResult> AppointFollowUp(NewFollowUpAppointmentDto appointmentDto)
        {
            var validationResult = await _newAppointmentFollowUpValidator.ValidateAsync(appointmentDto);
            if (validationResult.IsValid)
            {
                try
                {
                    await _appointmentService.AppointFollowUp(appointmentDto);
                    return Ok(new { Message = "Appointment Created Successfully" });
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("Time", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }

            }
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPost("video-call")]
        public async Task<IActionResult> AppointVideoCall(NewVideoCallAppointmentDto appointmentDto)
        {
            var validationResult = await _newAppointmentSurgeryValidator.ValidateAsync(appointmentDto);
            if (validationResult.IsValid)
            {
                try
                {
                    await _appointmentService.AppointVideoCall(appointmentDto);
                    return Ok(new { Message = "Appointment Created Successfully" });
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("Time", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }

            }
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpGet("reasons")]
        public IActionResult GetAppointmentReasons()
            => Ok(_appointmentService.AppointmentReasons());

        [HttpGet("/api/Appointments")]
        public async Task<IActionResult> GetAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null)
        {
            try
            {
                return Ok(await _appointmentService.GetAppointments(pageNumber, appointmentReason, appointmentStatus));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }



        [HttpGet("availble-times")]
        public async Task<IActionResult> GetAvailableSheduleTimes(string doctorId, string day)
        {
            try
            {
                return Ok(await _scheduleService.GetAvailableTimes(doctorId, day));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }

        }

        [HttpPut("accept-appointment")]
        public async Task<IActionResult> AcceptAppointment(Guid appointmentId)
        {
            try
            {
                await _appointmentService.AcceptAppointment(appointmentId);
                return Ok(new { Message = "Appointment Accepted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("reject-appointment")]
        public async Task<IActionResult> RejectAppointment(Guid appointmentId)
        {
            try
            {
                await _appointmentService.RejectAppointment(appointmentId);
                return Ok(new { Message = "Appointment Rejected Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

    }
}
