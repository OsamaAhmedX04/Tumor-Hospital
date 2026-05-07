//🟢 For SUCCESS test (happy path)

//Use this:

//Visa
//Card Number: 4005 5500 0000 0001
//Expiry: 12 / 26
//CVV: 100
//Name: Fawaterak test
//Expected result:
//Payment succeeds ✅
//User redirected to your success URL
//Webhook hits your API
//Donation → Paid
//CollectedAmount increases
//🔴 For FAILURE test (sad path)

//Use this:

//Visa
//Card Number: 4222 0000 0672 4235
//Expiry: 12 / 26
//CVV: 123
//Expected result:
//Payment fails ❌
//User redirected to failure URL
//Webhook hits your API
//Donation → Failed
//NO change in CollectedAmount

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

        [HttpGet("/success")]
        public IActionResult Success(string invoice_id)
        {
            return Ok(new
            {
                Status = "Success",
                Message = "Payment completed successfully",
                InvoiceId = invoice_id
            });
        }

        [HttpGet("/fail")]
        public IActionResult Fail(string invoice_id, string errorMessage)
        {
            return Ok(new
            {
                Status = "Failed",
                Message = "Payment failed",
                InvoiceId = invoice_id,
                Error = errorMessage
            });
        }

        [HttpGet("/pending")]
        public IActionResult Pending(string invoice_id)
        {
            return Ok(new
            {
                Status = "Pending",
                Message = "Payment is pending confirmation",
                InvoiceId = invoice_id
            });
        }

        [HttpPost("Donate")]
        public async Task<IActionResult> Donate(VolunteerDto dto)
        {
            try
            {
                var url = await _donationService.CreateDonation(dto);
                return Ok(new { PaymentUrl = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Donation failed",
                    details = ex.Message
                });
            }
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


        [HttpPost("fawatirak-webhook")]
        public async Task<IActionResult> Webhook([FromBody] WebHookModel model)
        {
            try
            {
                await _donationService.HandleWebhook(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
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