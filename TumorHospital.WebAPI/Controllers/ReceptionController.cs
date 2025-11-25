using Microsoft.AspNetCore.Http;
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

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        [HttpGet("Receptionists")]
        public async Task<IActionResult> GetAllReceptionists(int pageSize, int pageNumber, string? receptionistName)
        {
            try
            {
                var receptionists = await _receptionService
                    .GetAllReceptionists(pageSize, pageNumber, receptionistName);
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
        public async Task<IActionResult> GetAllBills(int pageSize, int pageNumber)
        {
            try
            {
                var bills = await _receptionService
                    .GetAllBills(pageSize, pageNumber);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpGet("bills/{patientEmail}")]
        public async Task<IActionResult> GetBill(int pageSize, int pageNumber, string patientEmail)
        {
            try
            {
                var bills = await _receptionService
                    .GetBill(pageSize, pageNumber, patientEmail);
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
                await _receptionService
                    .ReceivePayment(billId, receptionistId, billCode);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
