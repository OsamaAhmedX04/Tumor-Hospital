//Test Cards

//# Test Cards

//Here is some test cards to test Master card and Visa

//**This Card always return successful transaction:\&#xA;**

//Mastercard

//Card Number :5123450000000008\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26\
//CSV: 100

//Visa

//Card Number :4005 5500 0000 0001\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26\
//CSV: 100

//Meeza

//Card Number :5078 0362 4660 0381\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26\
//CSV: 100

//* *This Card always return Failed transaction:\&#xA;**

//Mastercard

//Card Number :5543474002249996\
//Card holder name : Fawaterak test\
//Expiry Date : 05 / 26
//CSV: 123

//Visa

//Card Number : 4222 0000 0672 4235\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26
//CSV: 123

//Meeza

//Card Number : 5078 0362 4278 3546\
//Card holder name : Fawaterak test\
//Expiry Date : 12 / 26
//CSV: 123
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Request.Payment;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Documentation;
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




        //[SwaggerOperation(Summary = DonationDocs.DonateSummary, Description = DonationDocs.DonateDescription)]
        //[HttpPost("Donate")]
        //public async Task<IActionResult> Donate(VolunteerDto volunteer)
        //{
        //    var validationResult = await _volunteerValidator.ValidateAsync(volunteer);

        //    if (validationResult.IsValid)
        //    {
        //        try
        //        {
        //            var paymentUrl = await _donationService.CreateDonation(volunteer);
        //            return Ok(new { PaymentUrl = paymentUrl });

        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new { Message = "An error occurred while processing the donation.", Details = ex.Message });
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

        //public async Task<IActionResult> Donate(VolunteerDto volunteer)
        //{
        //    var validationResult = await _volunteerValidator.ValidateAsync(volunteer);
        //    if (validationResult.IsValid)
        //    {
        //        try
        //        {
        //            await _donationService.Donate(volunteer);
        //            return Ok(new { Message = "Donation Added Successfully" });
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
                    var url = await _donationService.CreateDonation(dto);
                    return Ok(new { PaymentUrl = url });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                    //return StatusCode(500, new
                    //{
                    //    message = "Donation failed",
                    //    details = ex.Message
                    //});
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

        
        //[HttpPost("/fawatirak-webhook")]
        //public async Task<IActionResult> Webhook([FromBody] WebHookModel model)
        //{
        //    try
        //    {
        //        await _donationService.HandleWebhook(model);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
        //// 🔐 Verify request
        //if (!_paymentService.VerifyWebhook(model))
        //    return BadRequest("Invalid hash");

        //// extract OrderId from payload
        //var payload = JsonConvert.DeserializeObject<WebhookPayload>(model.PayloadString!);

        //var donationId = Guid.Parse(payload!.OrderId);

        //var donation = await _unitOfWork.VolunteerDonations.GetByIdAsync(donationId);

        //if (donation == null)
        //    return NotFound();

        //if (model.InvoiceStatus == "paid")
        //{
        //    donation.Status = "Paid";

        //    var need = await _unitOfWork.CharityNeeds.GetByIdAsync(donation.CharityNeedId.Value);

        //    need.CollectedAmount += donation.AmountDonated;

        //    if (need.CollectedAmount >= need.NeedAmount)
        //        need.IsCompleted = true;
        //}
        //else
        //{
        //    donation.Status = "Failed";
        //}

        //await _unitOfWork.CompleteAsync();

        //return Ok();


    }


}