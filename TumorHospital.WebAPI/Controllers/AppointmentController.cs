using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Request.Donation;
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

        public AppointmentController(IAppointmentService appointmentService, IValidator<NewConsultationAppointmentDto> newAppointmentConsultaionValidator)
        {
            _appointmentService = appointmentService;
            _newAppointmentConsultaionValidator = newAppointmentConsultaionValidator;
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
    }       
}
