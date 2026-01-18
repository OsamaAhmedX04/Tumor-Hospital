using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IReceptionService _receptionService;
        private readonly IBillSevice _billService;

        public ReceptionController(IReceptionService receptionService, IBillSevice billService)
        {
            _receptionService = receptionService;
            _billService = billService;
        }

        [HttpGet("Receptionists")]
        public async Task<IActionResult> GetAllReceptionists(int pageNumber, string? receptionistName)
        {
            try
            {
                var receptionists = await _receptionService
                    .GetAllReceptionists(pageNumber, receptionistName);
                return Ok(receptionists);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpGet("Receptionists/{id}")]
        public async Task<IActionResult> GetReceptionist(string id)
        {
            try
            {
                var receptionist = await _receptionService
                    .GetReceptionist(id);
                return Ok(receptionist);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [HttpGet("bills")]
        public async Task<IActionResult> GetBills(int pageNumber, string? patientEmail, string? patientName, string? billCode)
        {
            try
            {
                var bills = await _billService
                    .GetBills(pageNumber, patientEmail, patientName);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("Pay/{billId}")]
        public async Task<IActionResult> ReceivePayment(Guid billId, string receptionistId, string billCode)
        {
            try
            {
                await _billService
                    .ReceivePayment(billId, receptionistId, billCode);
                return Ok(new { Message = "Payment Received Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
