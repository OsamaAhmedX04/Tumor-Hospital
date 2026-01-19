using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            try
            {
                return Ok(await _specializationService.GetSpecializations());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

        [HttpGet("Specialization-names")]
        public async Task<IActionResult> GetSpecializationNames()
               => Ok(await _specializationService.GetSpecializationNames());

        [HttpPost]
        public async Task<IActionResult> AddSpecializations(SpecializationDto model)
        {
            try
            {
                await _specializationService.AddSpecialization(model);
                return Ok(new { Message = "Specialization Created Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecializations(Guid id, SpecializationDto model)
        {
            try
            {
                await _specializationService.UpdateSpecialization(id, model);
                return Ok(new { Message = "Specialization Updated Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecializations(Guid id)
        {
            try
            {
                await _specializationService.DeleteSpecialization(id);
                return Ok(new { Message = "Specialization Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
