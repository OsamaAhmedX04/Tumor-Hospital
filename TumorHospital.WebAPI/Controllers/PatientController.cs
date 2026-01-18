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
        private readonly IBillSevice _billService;
        public PatientController(IAppointmentService appointmentService, IBillSevice billService)
        {
            _appointmentService = appointmentService;
            _billService = billService;
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

        [HttpGet("Bills")]
        public async Task<IActionResult> GetBillss(int pageNumber, string patientId)
        {
            try
            {
                return Ok(await _billService.GetPatientBills(pageNumber, patientId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
