using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.FAQs;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Documentation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {

        private readonly IFAQSService _faqsService;
        private readonly IValidator<NewFAQsDto> _faqValidator;
        public FAQsController(IFAQSService faqsService, IValidator<NewFAQsDto> faqValidator)
        {
            _faqsService = faqsService;
            _faqValidator = faqValidator;
        }

        [SwaggerOperation(Summary = FAQsDocs.GetAllFAQsSummary, Description = FAQsDocs.GetAllFAQsDescription)]
        [HttpGet]
        public async Task<IActionResult> GetAllFAQs()
        {
            var faqs = await _faqsService.GetAllFAQs();
            return Ok(faqs);
        }


        [SwaggerOperation(Summary = FAQsDocs.AddFAQSummary, Description = FAQsDocs.AddFAQDescription)]
        [Authorize(Roles = SystemRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddFAQ([FromBody] NewFAQsDto dto)
        {
            var validation = _faqValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _faqsService.AddFAQ(dto);
                    return Ok(new { Message = "FAQ added successfully" });
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


        [SwaggerOperation(Summary = FAQsDocs.UpdateFAQSummary, Description = FAQsDocs.UpdateFAQDescription)]
        [Authorize(Roles = SystemRole.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] NewFAQsDto dto)
        {
            var validation = _faqValidator.Validate(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _faqsService.UpdateFAQ(id, dto);
                    return Ok(new { Message = "FAQ updated successfully" });
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


        [SwaggerOperation(Summary = FAQsDocs.DeleteFAQSummary, Description = FAQsDocs.DeleteFAQDescription)]
        [Authorize(Roles = SystemRole.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                await _faqsService.DeleteFAQ(id);
                return Ok(new { Message = "FAQ deleted successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
                return BadRequest(new { Errors = ModelState.ToErrorResponse() });
            }
        }
    }
}
