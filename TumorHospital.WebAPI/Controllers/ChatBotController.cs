using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.ML;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [ApiController]
    [Route("api/ChatBot")]
    public class ChatBotController : ControllerBase
    {
        private readonly IMLService _mlService;
        private readonly IValidator<ChatRequestDto> _chatValidator;

        public ChatBotController(IMLService mlService, IValidator<ChatRequestDto> chatValidator)
        {
            _mlService = mlService;
            _chatValidator = chatValidator;
        }

        [Authorize(Roles = SystemRole.Patient)]
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDto dto)
        {
            var validation = await _chatValidator.ValidateAsync(dto);

            if (validation.IsValid)
            {
                try
                {
                    var result = await _mlService.ChatAsync(dto);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("AIError", ex.Message);
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
