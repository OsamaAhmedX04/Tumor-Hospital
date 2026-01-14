using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.Intefaces.Services;

namespace TumorHospital.WebAPI.Controllers
{
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

        [HttpPost("CreatePrescription")]
        public async Task<IActionResult> Create(PrescriptionCreateUpdateDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("UpdatePrescription/{id}")]
        public async Task<IActionResult> Update(Guid id, PrescriptionCreateUpdateDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            return await _service.UpdateAsync(id, dto)
                ? Ok()
                : NotFound();
        }

        [HttpGet("GetPrescription/{appointmentId}")]
        public async Task<IActionResult> Get(Guid appointmentId)
            => Ok(await _service.GetByAppointmentIdAsync(appointmentId));

        [HttpDelete("DeletePrescription/{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => await _service.DeleteAsync(id) ? Ok() : NotFound();
    }
}
