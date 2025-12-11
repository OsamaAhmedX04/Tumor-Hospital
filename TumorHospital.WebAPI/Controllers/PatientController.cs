using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public PatientController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("Appointments")]
        public async Task<IActionResult> GetAppointments(int pageNumber, string patientId, string? appointmentReason = null, string? appointmentStatus = null)
        {
            try
            {
                return Ok(await _appointmentService.GetPatientAppointments(pageNumber, patientId, appointmentReason, appointmentStatus));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
