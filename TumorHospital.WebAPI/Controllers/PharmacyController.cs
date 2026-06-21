using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Pharmacy;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacyController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IValidator<NewPharmacyDto> _newPharmacyDtoValidator;
        private readonly IValidator<UpdatePharmacyDto> _updatePharmacyDtoValidator;

        public PharmacyController(IPharmacyService pharmacyService, IValidator<NewPharmacyDto> newPharmacyDtoValidator, IValidator<UpdatePharmacyDto> updatePharmacyDtoValidator)
        {
            _pharmacyService = pharmacyService;
            _newPharmacyDtoValidator = newPharmacyDtoValidator;
            _updatePharmacyDtoValidator = updatePharmacyDtoValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPharmacies()
        {
            var result = await _pharmacyService.GetAllPharmacies();
            return Ok(result);
        }


        [HttpGet("{pharmacyId}")]
        public async Task<IActionResult> GetPharmacy(Guid pharmacyId, int? year = null, int? month = null)
        {
            try
            {
                var result = await _pharmacyService.GetPharmacy(pharmacyId, year, month);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [HttpPost]
        public async Task<IActionResult> CreatePharmacy(NewPharmacyDto dto)
        {
            var validation = _newPharmacyDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _pharmacyService.CreatePharmacy(dto);
                    return Ok(new { Message = "New Pharmacy Created Successfully" });
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


        [HttpPut("{pharmacyId}")]
        public async Task<IActionResult> UpdatePharmacy(Guid pharmacyId, UpdatePharmacyDto dto)
        {
            var validation = _updatePharmacyDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _pharmacyService.UpdatePharmacy(pharmacyId, dto);
                    return Ok(new { Message = "Pharmacy Updated Successfully" });
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


        [HttpDelete("{pharmacyId}")]
        public async Task<IActionResult> DeletePharmacy(Guid pharmacyId)
        {
            try
            {
                await _pharmacyService.DeletePharmacy(pharmacyId);
                return Ok(new { Message = "Pharmacy Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
