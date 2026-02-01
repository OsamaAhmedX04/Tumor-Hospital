using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [EnableRateLimiting("strict")]
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly IValidator<PrescriptionCreateUpdateDto> _validator;

        public PrescriptionController(
            IPrescriptionService service,
            IValidator<PrescriptionCreateUpdateDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionCreateUpdateDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _service.CreateAsync(dto);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PrescriptionCreateUpdateDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(id, dto);
                    return Ok(new { Message = "New Prescription Has Updated Successfully" });
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

        [HttpGet("{appointmentId}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => await _service.DeleteAsync(id) ? Ok(new { Message = "Prescription has been Deleted" }) : NotFound(new { Message = "Prescription Not Found" });
    }
}
