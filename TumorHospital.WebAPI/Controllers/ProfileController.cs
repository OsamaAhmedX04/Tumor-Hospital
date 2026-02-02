using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Documentation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IValidator<UpdatePatientProfileDto> _patientValidator;
        private readonly IValidator<UpdateDoctorProfileDto> _doctorValidator;
        private readonly IValidator<UpdateReceptionistProfileDto> _receptionistValidator;

        public ProfileController(IProfileService profileSevice,
            IValidator<UpdatePatientProfileDto> patientValidator,
            IValidator<UpdateDoctorProfileDto> doctorValidator,
            IValidator<UpdateReceptionistProfileDto> receptionistValidator)
        {
            _profileService = profileSevice;
            _patientValidator = patientValidator;
            _doctorValidator = doctorValidator;
            _receptionistValidator = receptionistValidator;
        }

        [SwaggerOperation(Summary = ProfileDocs.GetPatientProfileSummary, Description = ProfileDocs.GetPatientProfileDescription)]
        //[Authorize(Roles = SystemRole.Patient)]
        [HttpGet("PatientProfile/{userId}")]
        public async Task<IActionResult> GetPatientProfile(string userId)
        {
            var result = await _profileService.GetPatientProfile(userId);
            if (result == null) return NotFound(new { Message = "User Not Found" });

            return Ok(result);
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdatePatientProfileSummary, Description = ProfileDocs.UpdatePatientProfileDescription)]
        //[Authorize(Roles = SystemRole.Patient)]
        [HttpPut("PatientProfile/{userId}")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdatePatientProfileDto dto)
        {
            var validationResult = await _patientValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(userId, dto);
                return Ok("Patient profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [SwaggerOperation(Summary = ProfileDocs.GetDoctorProfileSummary, Description = ProfileDocs.GetDoctorProfileDescription)]
        //[Authorize(Roles = SystemRole.Doctor)]
        [HttpGet("DoctorProfile/{userId}")]
        public async Task<IActionResult> GetDoctorProfile(string userId)
        {
            var result = await _profileService.GetDoctorProfile(userId);
            if (result == null) return NotFound(new { Message = "User Not Found" });

            return Ok(result);
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdateDoctorProfileSummary, Description = ProfileDocs.UpdateDoctorProfileDescription)]
        //[Authorize(Roles = SystemRole.Doctor)]
        [HttpPut("DoctorProfile/{userId}")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateDoctorProfileDto dto)
        {
            var validationResult = await _doctorValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(userId, dto);
                return Ok("Doctor profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [SwaggerOperation(Summary = ProfileDocs.GetReceptionistProfileSummary, Description = ProfileDocs.GetReceptionistProfileDescription)]
        //[Authorize(Roles = SystemRole.Receptionist)]
        [HttpGet("ReceptionistProfile/{userId}")]
        public async Task<IActionResult> GetReceptionistProfile(string userId)
        {
            var result = await _profileService.GetReceptionistProfile(userId);
            if (result == null) return NotFound(new { Message = "User Not Found" });

            return Ok(result);
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdateReceptionistProfileSummary, Description = ProfileDocs.UpdateReceptionistProfileDescription)]
        //[Authorize(Roles = SystemRole.Receptionist)]
        [HttpPut("ReceptionistProfile/{userId}")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateReceptionistProfileDto dto)
        {
            var validationResult = await _receptionistValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(userId, dto);
                return Ok("Receptionist profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
