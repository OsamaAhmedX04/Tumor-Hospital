using Microsoft.AspNetCore.Http;
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
        public DoctorController(IProfileService profileService, IDoctorService doctorService)
        {
            _profileService = profileService;
            _doctorService = doctorService;
        }

        [HttpPost("Profile-Picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, string userId)
        {
            try
            {
                await _profileService.UploadProfilePicture(file, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

        [HttpGet("/api/Doctors")]
        public async Task<IActionResult> GetDoctors(int pageSize, int pageNumber, string? workDay = null)
        {
            try
            {
                return Ok(await _doctorService.GetDoctors(pageSize, pageNumber, workDay));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctors(string id)
        {
            try
            {
                return Ok(await _doctorService.GetDoctorDetails(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
