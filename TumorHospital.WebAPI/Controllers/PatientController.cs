using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Documentation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = SystemRole.Patient)]
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
        [HttpGet("Appointments")]
        public async Task<IActionResult> GetAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null)
        {
            try
            {
                return Ok(await _appointmentService.GetPatientAppointments(pageNumber, appointmentReason, appointmentStatus));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }


        [SwaggerOperation(Summary = PatientDocs.GetBillsSummary, Description = PatientDocs.GetBillsDescription)]
        [HttpGet("Bills")]
        public async Task<IActionResult> GetBills(int pageNumber)
        {
            try
            {
                return Ok(await _billService.GetPatientBills(pageNumber));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
