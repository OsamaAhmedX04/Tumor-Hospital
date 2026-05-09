using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Request.Payment;
using TumorHospital.Application.Intefaces.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]/fawaterak/webhooks")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public PaymentController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpPut("success")]
        public async Task<IActionResult> Success(string invoice_id)
        {
            var amount = await _donationService.SuccessDonation(invoice_id);
            return Ok(new
            {
                Status = "Success",
                Message = "Payment completed successfully",
                InvoiceId = invoice_id,
                Amount = amount
            });
        }

        [HttpPut("fail")]
        public async Task<IActionResult> Fail(string invoice_id, string errorMessage)
        {
            await _donationService.FaildedDonation(invoice_id);

            return Ok(new
            {
                Status = "Failed",
                Message = "Payment failed",
                InvoiceId = invoice_id,
                Error = errorMessage
            });
        }

        [HttpPut("pending")]
        public IActionResult Pending(string invoice_id)
        {
            return Ok(new
            {
                Status = "Pending",
                Message = "Payment is pending confirmation",
                InvoiceId = invoice_id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Webhook([FromBody] WebHookModel model)
        {
            try
            {
                //Console.WriteLine("WEBHOOK HIT");

                await _donationService.HandleWebhook(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }

    }
}
