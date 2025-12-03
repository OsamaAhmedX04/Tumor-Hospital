using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IValidator<NewConsultationAppointmentDto> _newAppointmentConsultaionValidator;
        private readonly IValidator<AppointmentSetterDateTimeDto> _appointmentSetterDateTimeDtoValidator;

        public AppointmentController(
            IAppointmentService appointmentService,
            IValidator<NewConsultationAppointmentDto> newAppointmentConsultaionValidator,
            IValidator<AppointmentSetterDateTimeDto> appointmentSetterDateTimeDtoValidator
            )
        {
            _appointmentService = appointmentService;
            _newAppointmentConsultaionValidator = newAppointmentConsultaionValidator;
            _appointmentSetterDateTimeDtoValidator = appointmentSetterDateTimeDtoValidator;
        }

        [HttpPost("Consultaion")]
        public async Task<IActionResult> AppointConsultation(NewConsultationAppointmentDto appointmentDto)
        {
            var validationResult = await _newAppointmentConsultaionValidator.ValidateAsync(appointmentDto);
            if (validationResult.IsValid)
            {
                try
                {
                    await _appointmentService.AppointConsultation(appointmentDto);
                    return Ok();
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

        [HttpGet]
        public async Task<IActionResult> GetAppointments(int pageNumber)
            => Ok(await _appointmentService.GetAppointments(pageNumber));

        [HttpPut("Accept-Appointment")]
        public async Task<IActionResult> AcceptAppointment(Guid appointmentId, [FromForm]AppointmentSetterDateTimeDto setter)
        {
            var validationResult = await _appointmentSetterDateTimeDtoValidator.ValidateAsync(setter);
            if (validationResult.IsValid)
            {
                try
                {
                    await _appointmentService.AcceptAppointment(appointmentId, setter);
                    return Ok();
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
        [HttpPut("Reject-Appointment")]
        public async Task<IActionResult> RejectAppointment(Guid appointmentId)
        {
            try
            {
                await _appointmentService.RejectAppointment(appointmentId);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
