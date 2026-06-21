using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Pharmacy;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacistController : ControllerBase
    {
        private readonly IPharmacistService _pharmacistService;
        private readonly IValidator<NewPharmacistDto> _newPharmacistDtoValidator;

        public PharmacistController(IPharmacistService pharmacistService, IValidator<NewPharmacistDto> newPharmacistDtoValidator)
        {
            _pharmacistService = pharmacistService;
            _newPharmacistDtoValidator = newPharmacistDtoValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePharmacist(NewPharmacistDto dto)
        {
            var validation = _newPharmacistDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _pharmacistService.CreatePharmacist(dto);
                    return Ok(new { Message = "New Pharmacist Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [HttpDelete("{pharmacistId}")]
        public async Task<IActionResult> DeletePharmacist(string pharmacistId)
        {
            try
            {
                await _pharmacistService.DeletePharmacist(pharmacistId);
                return Ok(new { Message = "Pharmacist Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
