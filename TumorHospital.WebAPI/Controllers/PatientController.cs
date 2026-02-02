using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Documentation;
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


        [SwaggerOperation(Summary = PatientDocs.GetAppointmentsSummary, Description = PatientDocs.GetAppointmentsDescription)]
        //[Authorize(Roles = SystemRole.Patient)]
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


        [SwaggerOperation(Summary = PatientDocs.GetBillsSummary, Description = PatientDocs.GetBillsDescription)]
        //[Authorize(Roles = SystemRole.Patient)]
        [HttpGet("Bills")]
        public async Task<IActionResult> GetBills(int pageNumber, string patientId)
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
