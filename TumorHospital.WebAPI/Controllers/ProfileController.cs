using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Infrastructure.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileSevice)
        {
            _profileService = profileSevice;
        }

        [HttpGet("GetPatientProfile/{userId}")]
        public async Task<IActionResult> GetPatientProfile(string userId)
        {
            var result = await _profileService.GetPatientProfile(userId);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("UpdatePatientProfile/{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdatePatientProfileDto dto)
        {
            var success = await _profileService.UpdateProfile(userId, dto);
            if (!success) return NotFound();

            return Ok("Patient profile updated successfully");
        }

        [HttpGet("GetDoctorProfile/{userId}")]
        public async Task<IActionResult> GetDoctorProfile(string userId)
        {
            var result = await _profileService.GetDoctorProfile(userId);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPut("UpdateDoctorProfile/{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateDoctorProfileDto dto)
        {
            var success = await _profileService.UpdateProfile(userId, dto);
            if (!success) return NotFound();

            return Ok("Doctor profile updated successfully");
        }
    }
}
