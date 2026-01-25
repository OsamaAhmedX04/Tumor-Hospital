using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }


        [SwaggerOperation(Summary = SpecializationDocs.GetSpecializationsSummary, Description = SpecializationDocs.GetSpecializationsDescription)]
        [Authorize(Roles = SystemRole.Admin)]
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



        [SwaggerOperation(Summary = SpecializationDocs.GetSpecializationNamesSummary, Description = SpecializationDocs.GetSpecializationNamesDescription)]
        [HttpGet("Specialization-names")]
        public async Task<IActionResult> GetSpecializationNames()
               => Ok(await _specializationService.GetSpecializationNames());



        [SwaggerOperation(Summary = SpecializationDocs.AddSpecializationsSummary, Description = SpecializationDocs.AddSpecializationsDescription)]
        [Authorize(Roles = SystemRole.Admin)]
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



        [SwaggerOperation(Summary = SpecializationDocs.UpdateSpecializationsSummary, Description = SpecializationDocs.UpdateSpecializationsDescription)]
        [Authorize(Roles = SystemRole.Admin)]
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



        [SwaggerOperation(Summary = SpecializationDocs.DeleteSpecializationsSummary, Description = SpecializationDocs.DeleteSpecializationsDescription)]
        [Authorize(Roles = SystemRole.Admin)]
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
