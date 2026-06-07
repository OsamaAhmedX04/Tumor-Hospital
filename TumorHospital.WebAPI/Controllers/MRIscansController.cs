using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.ML;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [ApiController]
    [Route("api/MRIscan")]
    public class MRIscansController : ControllerBase
    {
        private readonly IMLService _mlService;
        private readonly IValidator<ExplainRequestDto> _explainValidator;

        public MRIscansController(IMLService mlService, IValidator<ExplainRequestDto> explainValidator)
        {
            _mlService = mlService;
            _explainValidator = explainValidator;
        }

        [HttpPost("explain")]
        public async Task<IActionResult> Explain([FromForm] ExplainRequestDto dto)
        {
            var validation =
                await _explainValidator.ValidateAsync(dto);

            if (validation.IsValid)
            {
                try
                {
                    var result = await _mlService.ExplainAsync(dto);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(
                        "AIError",
                        ex.Message);
                }
            }

            foreach (var error in validation.Errors)
                ModelState.AddModelError(
                    error.PropertyName,
                    error.ErrorMessage);

            return BadRequest(new
            {
                Errors = ModelState.ToErrorResponse()
            });
        }
    }
}
