using Amazon.Runtime.Internal.Util;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Request.About_Contact;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Validators.About;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [ApiController]
    [Route("api/about")]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        private readonly IValidator<AddAboutInfoDto> _validator;

        public AboutController(IAboutService aboutService, IValidator<AddAboutInfoDto> validator)
        {
            _aboutService = aboutService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _aboutService.GetAboutAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(AddAboutInfoDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _aboutService.AddOrUpdateAsync(dto);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _aboutService.DeleteAsync(id);
            return NoContent();
        }
    }
}
