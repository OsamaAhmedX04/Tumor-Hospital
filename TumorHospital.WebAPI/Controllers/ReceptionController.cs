using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
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

        [HttpGet("/api/Receptionists")]
        [Authorize(Roles = SystemRole.Admin)]
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

        [HttpGet("{id}")]
        [Authorize(Roles = SystemRole.Admin)]
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
        [Authorize(Roles = SystemRole.Receptionist)]
        public async Task<IActionResult> GetBills(int pageNumber, string? patientEmail, string? patientName, string? billCode, int? month, int? year)
        {
            try
            {
                var bills = await _billService
                    .GetBills(pageNumber, patientEmail, patientName, billCode, month, year);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("Pay/{billId}")]
        [Authorize(Roles = SystemRole.Receptionist)]
        public async Task<IActionResult> ReceivePayment(Guid billId, string billCode)
        {
            try
            {
                await _billService
                    .ReceivePayment(billId, billCode);
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
