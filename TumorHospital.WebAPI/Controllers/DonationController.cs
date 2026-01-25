using FluentValidation;
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
        private readonly IValidator<VolunteerDto> _volunteerValidator;

        public DonationController(IDonationService donationService, IValidator<VolunteerDto> volunteerValidator)
        {
            _donationService = donationService;
            _volunteerValidator = volunteerValidator;
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
                    return Ok(new { Message = "Donation Added Successfully" });
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
    }
}
