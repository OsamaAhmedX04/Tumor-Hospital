using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Supply;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IValidator<NewSupplierDto> _newSupplierDtoValidator;
        private readonly IValidator<UpdateSupplierDto> _updateSupplierDtoValidator;

        public SupplierController(ISupplierService supplierService, IValidator<NewSupplierDto> newSupplierDtoValidator, IValidator<UpdateSupplierDto> updateSupplierDtoValidator)
        {
            _supplierService = supplierService;
            _newSupplierDtoValidator = newSupplierDtoValidator;
            _updateSupplierDtoValidator = updateSupplierDtoValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers(int pageNumber, string? name = null)
        {
            var result = await _supplierService.GetAllSuppliers(pageNumber, name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(NewSupplierDto dto)
        {
            var validation = _newSupplierDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _supplierService.CreateSupplier(dto);
                    return Ok(new { Message = "New Supplier Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(Guid supplierId, UpdateSupplierDto dto)
        {
            var validation = _updateSupplierDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _supplierService.UpdateSupplier(supplierId, dto);
                    return Ok(new { Message = "Supplier Updated Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(Guid supplierId)
        {
            try
            {
                await _supplierService.DeleteSupplier(supplierId);
                return Ok(new { Message = "Supplier Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

    }
}
