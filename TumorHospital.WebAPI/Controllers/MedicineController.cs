using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Medicine;
using TumorHospital.Application.DTOs.Request.Supply;
using TumorHospital.Application.DTOs.Response.Medicine;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IValidator<NewMedicineDto> _newMedicineDtoValidator;
        private readonly IValidator<UpdateMedicineDto> _updateMedicineDtoValidator;

        public MedicineController(IMedicineService medicineService, IValidator<NewMedicineDto> newMedicineDtoValidator, IValidator<UpdateMedicineDto> updateMedicineDtoValidator)
        {
            _medicineService = medicineService;
            _newMedicineDtoValidator = newMedicineDtoValidator;
            _updateMedicineDtoValidator = updateMedicineDtoValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicines(int pageNumber, string? name = null)
        {
            var result = await _medicineService.GetMedicines(pageNumber, name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicine(NewMedicineDto dto)
        {
            var validation = _newMedicineDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _medicineService.CreateMedicine(dto);
                    return Ok(new { Message = "New Medicine Created Successfully" });
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

        [HttpPut("{medicineId}")]
        public async Task<IActionResult> UpdateMedicine(Guid medicineId, UpdateMedicineDto dto)
        {
            var validation = _updateMedicineDtoValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _medicineService.UpdateMedicine(medicineId ,dto);
                    return Ok(new { Message = "Medicine Updated Successfully" });
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

        [HttpDelete("{medicineId}")]
        public async Task<IActionResult> DeleteMedicine(Guid medicineId)
        {
            try
            {
                await _medicineService.DeleteMedicine(medicineId);
                return Ok(new { Message = "Medicine Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("assign/{medicineId}/{supplierId}")]
        public async Task<IActionResult> ReassignMedicineToSupplier(Guid medicineId, Guid supplierId)
        {
            try
            {
                await _medicineService.ReassignMedicineToSupplier(medicineId, supplierId);
                return Ok(new { Message = "Medicine assigned Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("sell/{medicineId}")]
        public async Task<IActionResult> SellMedicine(Guid medicineId, int quantity)
        {
            try
            {
                await _medicineService.SellMedicine(medicineId, quantity);
                return Ok(new { Message = "Medicine has been sold Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
