using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [EnableRateLimiting("strict")]
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly IValidator<PrescriptionUpdateDto> _updateValidator;
        private readonly IValidator<PrescriptionCreateDto> _createValidator;

        public PrescriptionController(
            IPrescriptionService service,
            IValidator<PrescriptionUpdateDto> updateValidator, IValidator<PrescriptionCreateDto> createValidator)
        {
            _service = service;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        [HttpGet("{appointmentId}")]
        [Authorize(Roles = SystemRole.Doctor + "," + SystemRole.Patient)]
        public async Task<IActionResult> Get(Guid appointmentId)
        {
            try
            {
                return Ok(await _service.GetByAppointmentIdAsync(appointmentId));
            }

            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("{appointmentId}")]
        [Authorize(Roles = SystemRole.Doctor)]
        public async Task<IActionResult> Create(Guid appointmentId, PrescriptionCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _service.CreateAsync(appointmentId, dto);
                    return Ok(new { Message = "New Prescription Has Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("DateConflict", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("{prescriptionId}")]
        [Authorize(Roles = SystemRole.Doctor)]
        public async Task<IActionResult> Update(Guid prescriptionId, PrescriptionUpdateDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(prescriptionId, dto);
                    return Ok(new { Message = "Prescription Has Updated Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("DateConflict", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpDelete("{prescriptionid}")]
        [Authorize(Roles = SystemRole.Doctor)]
        public async Task<IActionResult> Delete(Guid prescriptionid)
        {
            try
            {
                await _service.DeleteAsync(prescriptionid);
                return Ok(new { Message = "Prescription has been Deleted" });
            }

            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
