//Test Cards
//Mastercard
//Card Number :5123450000000008\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26\
//CSV: 100

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


        //[HttpPost("Donate")]
        //public async Task<IActionResult> Donate(VolunteerDto dto)
        //{
        //    var validationResult = await _volunteerValidator.ValidateAsync(dto);
        //    if (validationResult.IsValid)
        //    {
        //        try
        //        {
        //            var url = await _donationService.CreateDonation(dto);
        //            return Ok(new { PaymentUrl = url });
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("Message", ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var error in validationResult.Errors)
        //        {
        //            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        //        }
        //    }
        //    return BadRequest(new { Errors = ModelState.ToErrorResponse() });

        //}

        [HttpPost("Donate")]
        public async Task<IActionResult> Donate(VolunteerDto dto)
        {
            var validationResult = await _volunteerValidator.ValidateAsync(dto);
            if (validationResult.IsValid)
            {
                try
                {
                    await _donationService.Donate(dto);
                    return Ok(new { Message = "Donation Created Successfully" });
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