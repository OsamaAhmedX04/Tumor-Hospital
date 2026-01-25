using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Documentation.Authentication;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        public DoctorController(IProfileService profileService, IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _profileService = profileService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [SwaggerOperation(Summary = DoctorDocs.UploadProfilePictureSummary, Description = DoctorDocs.UploadProfilePictureDescription)]
        [Authorize(Roles = SystemRole.Doctor)]
        [HttpPost("Profile-Picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, string userId)
        {
            try
            {
                await _profileService.UploadProfilePicture(file, userId);
                return Ok(new { Message = "Profile Picture Uploaded Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }


        [SwaggerOperation(Summary = DoctorDocs.GetDoctorsSummary, Description = DoctorDocs.GetDoctorsDescription)]
        [HttpGet("/api/Doctors")]
        public async Task<IActionResult> GetDoctors(int pageNumber, string? workDay = null, bool? IsSurgeon = null, string? government = null)
        {
            try
            {
                return Ok(await _doctorService.GetDoctors(pageNumber, workDay, IsSurgeon, government));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }


        [SwaggerOperation(Summary = DoctorDocs.GetDoctorSummary, Description = DoctorDocs.GetDoctorDescription)]
        [Authorize(Roles =SystemRole.Patient)]
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctor(string doctorId, string patientId)
        {
            try
            {
                return Ok(await _doctorService.GetDoctorDetails(doctorId, patientId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }


        [SwaggerOperation(Summary = DoctorDocs.GetDoctorAppointmentsSummary, Description = DoctorDocs.GetDoctorAppointmentsDescription)]
        [Authorize(Roles = SystemRole.Doctor)]
        [HttpGet("Appointments")]
        public async Task<IActionResult> GetDoctorAppointments(int pageNumber, string doctorId, string? appointmentReason = null, string? appointmentStatus = null)
        {
            try
            {
                return Ok(await _appointmentService.GetDoctorAppointments(pageNumber, doctorId, appointmentReason, appointmentStatus));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
