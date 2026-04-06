using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.About_Contact;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [ApiController]
    [Route("api/about")]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        private readonly IValidator<AddAboutInfoDto> _addValidator;
        private readonly IValidator<UpdateAboutInfoDto> _updateValidator;

        public AboutController(IAboutService aboutService, IValidator<AddAboutInfoDto> addValidator, IValidator<UpdateAboutInfoDto> updateValidator)
        {
            _aboutService = aboutService;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _aboutService.GetAboutAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAboutInfoDto dto)
        {
            var validation = await _addValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _aboutService.AddAsync(dto);
                    return Ok(new { Message = "New About Has Registerd Successfully" });
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
        public async Task<IActionResult> Update(Guid id, UpdateAboutInfoDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _aboutService.UpdateAsync(id, dto);
                    return Ok(new { Message = "About Has Updated Successfully" });
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _aboutService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("NotFound", ex.Message);
                return NotFound(new { Errors = ModelState.ToErrorResponse() });
            }
            return Ok(new { Message = "About Has Deleted Successfully" });
        }
    }
}
