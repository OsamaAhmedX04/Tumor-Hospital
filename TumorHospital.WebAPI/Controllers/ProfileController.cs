using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
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
        private readonly IValidator<UpdatePharmacistProfileDto> _pharmacistValidator;

        public ProfileController(IProfileService profileSevice,
            IValidator<UpdatePatientProfileDto> patientValidator,
            IValidator<UpdateDoctorProfileDto> doctorValidator,
            IValidator<UpdateReceptionistProfileDto> receptionistValidator,
            IValidator<UpdatePharmacistProfileDto> pharmacistValidator)
        {
            _profileService = profileSevice;
            _patientValidator = patientValidator;
            _doctorValidator = doctorValidator;
            _receptionistValidator = receptionistValidator;
            _pharmacistValidator = pharmacistValidator;
        }

        [SwaggerOperation(Summary = ProfileDocs.GetPatientProfileSummary, Description = ProfileDocs.GetPatientProfileDescription)]
        [Authorize(Roles = SystemRole.Patient)]
        [HttpGet("Patient")]
        public async Task<IActionResult> GetPatientProfile()
        {
            try
            {
                var result = await _profileService.GetPatientProfile();
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = ProfileDocs.GetDoctorProfileSummary, Description = ProfileDocs.GetDoctorProfileDescription)]
        [Authorize(Roles = SystemRole.Doctor)]
        [HttpGet("Doctor")]
        public async Task<IActionResult> GetDoctorProfile()
        {
            try
            {
                var result = await _profileService.GetDoctorProfile();
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = ProfileDocs.GetReceptionistProfileSummary, Description = ProfileDocs.GetReceptionistProfileDescription)]
        [Authorize(Roles = SystemRole.Receptionist)]
        [HttpGet("Receptionist")]
        public async Task<IActionResult> GetReceptionistProfile()
        {
            try
            {
                var result = await _profileService.GetReceptionistProfile();
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [Authorize(Roles = SystemRole.Pharmacist)]
        [HttpGet("Pharmacist")]
        public async Task<IActionResult> GetPharmacistProfile()
        {
            try
            {
                var result = await _profileService.GetPharmacistProfile();
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdatePatientProfileSummary, Description = ProfileDocs.UpdatePatientProfileDescription)]
        [Authorize(Roles = SystemRole.Patient)]
        [HttpPut("Patient")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(UpdatePatientProfileDto dto)
        {
            var validationResult = await _patientValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(dto);
                return Ok("Patient profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdateDoctorProfileSummary, Description = ProfileDocs.UpdateDoctorProfileDescription)]
        [Authorize(Roles = SystemRole.Doctor)]
        [HttpPut("Doctor")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(UpdateDoctorProfileDto dto)
        {
            var validationResult = await _doctorValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(dto);
                return Ok("Doctor profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [SwaggerOperation(Summary = ProfileDocs.UpdateReceptionistProfileSummary, Description = ProfileDocs.UpdateReceptionistProfileDescription)]
        [Authorize(Roles = SystemRole.Receptionist)]
        [HttpPut("Receptionist")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(UpdateReceptionistProfileDto dto)
        {
            var validationResult = await _receptionistValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(dto);
                return Ok("Receptionist profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [Authorize(Roles = SystemRole.Pharmacist)]
        [HttpPut("Pharmacist")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> UpdateProfile(UpdatePharmacistProfileDto dto)
        {
            var validationResult = await _pharmacistValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                await _profileService.UpdateProfile(dto);
                return Ok("Pharmacist profile updated successfully");
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
