using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Offer;
using TumorHospital.Application.DTOs.Response.Offer;
using TumorHospital.Application.Intefaces.Services;
using FluentValidation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IValidator<AddOfferDto> _addValidator;
        private readonly IValidator<UpdateOfferDto> _updateValidator;

        public OffersController(IOfferService offerService, IValidator<AddOfferDto> addValidator,
        IValidator<UpdateOfferDto> updateValidator)
        {
            _offerService = offerService;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost("AddOffer")]
        public async Task<IActionResult> AddOffer(AddOfferDto dto)
        {
            var validation = await _addValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _offerService.AddOfferAsync(dto);
                    return Ok(new { Message = "New Offer Has Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("DateConflict", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("UpdateOffer/{id}")]
        public async Task<IActionResult> UpdateOffer(Guid id, UpdateOfferDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (validation.IsValid)
            {
                try
                {
                    await _offerService.UpdateOfferAsync(id, dto);
                    return Ok(new { Message = "Offer Updated Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("DateConflict", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpDelete("DeleteOffer/{id}")]
        public async Task<IActionResult> DeleteOffer(Guid id)
        {
            var success = await _offerService.RemoveOfferAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("GetOffers")]
        public async Task<IActionResult> GetOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }

        [HttpGet("GetExpiredOffers")]
        public async Task<IActionResult> GetExpired()
        {
            var offers = await _offerService.GetExpiredOffersAsync();
            return Ok(offers);
        }

        [HttpGet("GetUpcomingOffers")]
        public async Task<IActionResult> GetUpcoming()
        {
            var offers = await _offerService.GetUpcomingOffersAsync();
            return Ok(offers);
        }
    }
}
