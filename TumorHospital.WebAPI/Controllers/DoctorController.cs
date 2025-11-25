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
        public DoctorController(IProfileService profileService)
        {
            _profileService = profileService;
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
                ModelState.AddModelError("Identity", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
