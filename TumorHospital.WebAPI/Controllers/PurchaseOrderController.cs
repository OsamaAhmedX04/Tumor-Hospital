using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequestsOrderToSuppliers(int pageNumber, string? medicineName = null, string? status = null, string? supplierName = null)
        {
            try
            {
                var result = await _purchaseOrderService.GetAllRequestsOrderToSuppliers(pageNumber, medicineName, status, supplierName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPost]
        public async Task<IActionResult> RequestOrderToSupplier(Guid supplierId, Guid medicineId, int quantity)
        {
            try
            {
                await _purchaseOrderService.RequestOrderToSupplier(supplierId, medicineId, quantity);
                return Ok(new { Message = "Request has been Sent Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("accept/{orderId}")]
        public async Task<IActionResult> CompleteRequestedOrder(int orderId)
        {
            try
            {
                await _purchaseOrderService.CompleteRequestedOrder(orderId);
                return Ok(new { Message = "Request has been Completed Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("decline/{orderId}")]
        public async Task<IActionResult> DeclineRequestedOrder(int orderId)
        {
            try
            {
                await _purchaseOrderService.DeclineRequestedOrder(orderId);
                return Ok(new { Message = "Request has been Declined Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

    }
}
