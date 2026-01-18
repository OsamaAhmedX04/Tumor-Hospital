using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;
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

        [HttpGet("/api/Doctors")]
        public async Task<IActionResult> GetDoctors(int pageNumber, string? workDay = null, bool? IsSurgeon = null)
        {
            try
            {
                return Ok(await _doctorService.GetDoctors(pageNumber, workDay, IsSurgeon));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

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

        [HttpGet("Appointments")]
        public async Task<IActionResult> GetAppointments(int pageNumber, string doctorId, string? appointmentReason = null, string? appointmentStatus = null)
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
