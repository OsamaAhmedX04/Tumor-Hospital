using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

// NOT TESTED YET!

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NeedController : ControllerBase
    {
        private readonly INeedService _needService;
        private readonly IValidator<NewNeedDto> _newNeedValidator;
        private readonly IValidator<UpdateNeedDto> _updateNeedValidator;
        public NeedController(
            INeedService needService,
            IValidator<NewNeedDto> newNeedValidator,
            IValidator<UpdateNeedDto> updateNeedValidator)
        {
            _needService = needService;
            _newNeedValidator = newNeedValidator;
            _updateNeedValidator = updateNeedValidator;
        }

        [HttpGet("needs")]
        public async Task<IActionResult> GetAllNeeds(int pageSize, int pageNumber)
        {
            var needs = await _needService.GetAllNeeds(pageSize, pageNumber);
            return Ok(needs);
        }



        [HttpGet("need/{id}")]
        public async Task<IActionResult> GetNeed([FromQuery] Guid id)
        {
            try
            {
                var need = await _needService.GetNeed(id);
                return Ok(need);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [HttpPost]
        public async Task<IActionResult> AddNeed(NewNeedDto need)
        {
            var validationResult = await _newNeedValidator.ValidateAsync(need);
            if (validationResult.IsValid)
            {
                await _needService.AddNeed(need);
                return Ok(new { Message = "New Need Created Successfully" });
            }

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [HttpPut]
        public async Task<IActionResult> UpdateNeed([FromForm] UpdateNeedDto newNeed, Guid id)
        {
            var validationResult = await _updateNeedValidator.ValidateAsync(newNeed);
            if (validationResult.IsValid)
            {
                try
                {
                    await _needService.UpdateNeed(newNeed, id);
                    return Ok(new { Message = "Need Updated Successfully" });
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNeed(Guid id)
        {
            try
            {
                await _needService.DeleteNeed(id);
                return Ok(new { Message = "Need Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }



        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesOfNeeds() => Ok(_needService.GetCategoriesOfNeeds());








    }
}
