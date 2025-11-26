using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        private readonly IValidator<NewNeedDto> _newNeedValidator;
        private readonly IValidator<VolunteerDto> _volunteerValidator;
        private readonly IValidator<UpdateNeedDto> _updateNeedValidator;
        public DonationController(
            IDonationService donationService,
            IValidator<NewNeedDto> newNeedValidator,
            IValidator<VolunteerDto> volunteerValidator,
            IValidator<UpdateNeedDto> updateNeedValidator)
        {
            _donationService = donationService;
            _newNeedValidator = newNeedValidator;
            _volunteerValidator = volunteerValidator;
            _updateNeedValidator = updateNeedValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNeeds(int pageSize, int pageNumber)
        {
            var needs = await _donationService.GetAllNeeds(pageSize, pageNumber);
            return Ok(needs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNeed([FromQuery] Guid id)
        {
            try
            {
                var need = await _donationService.GetNeed(id);
                return Ok(need);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesOfNeeds() => Ok(_donationService.GetCategoriesOfNeeds());

        [HttpGet("Volunteers")]
        public async Task<IActionResult> GetAllVolunteers(int pageSize, int pageNumber)
        {
            var volunteers = await _donationService.GetAllVolunteers(pageSize, pageNumber);
            return Ok(volunteers);
        }

        [HttpPost]
        public async Task<IActionResult> AddNeed(NewNeedDto need)
        {
            var validationResult = await _newNeedValidator.ValidateAsync(need);
            if (validationResult.IsValid)
            {
                await _donationService.AddNeed(need);
                return Ok();
            }
            
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
        [HttpPost("Donate")]
        public async Task<IActionResult> Donate(VolunteerDto volunteer)
        {
            var validationResult = await _volunteerValidator.ValidateAsync(volunteer);
            if (validationResult.IsValid)
            {
                try
                {
                    await _donationService.Donate(volunteer);
                    return Ok();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNeed([FromForm] UpdateNeedDto newNeed, Guid id)
        {
            var validationResult = await _updateNeedValidator.ValidateAsync(newNeed);
            if (validationResult.IsValid)
            {
                try
                {
                    await _donationService.UpdateNeed(newNeed, id);
                    return Ok();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNeed(Guid id)
        {
            try
            {
                await _donationService.DeleteNeed(id);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
