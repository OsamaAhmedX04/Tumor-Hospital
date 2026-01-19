using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.Hospital;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;
        private readonly IValidator<HospitalDto> _hospitalValidator;

        public HospitalController(IHospitalService hospitalService, IValidator<HospitalDto> hospitalValidator)
        {
            _hospitalService = hospitalService;
            _hospitalValidator = hospitalValidator;
        }


        [HttpGet("/api/Hospitals")]
        public async Task<IActionResult> GetAllHospitals()
        {
            return Ok(await _hospitalService.GetHospitals());
        }

        [HttpGet("{hospitalId}/doctors")]
        public async Task<IActionResult> GetAllHospitalDoctors(Guid hospitalId, string doctorName, int pageNumber)
        {
            try
            {
                var doctors = await _hospitalService.GetHospitalDoctors(hospitalId, doctorName, pageNumber);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("System", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAllHospitalDoctors(string doctorId)
        {
            try
            {
                var doctors = await _hospitalService.GetHospitalDoctor(doctorId);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("System", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [HttpGet("{hospitalId}/receptionists")]
        public async Task<IActionResult> GetAllHospitalReceptionists(Guid hospitalId, string receptionistName, int pageNumber)
        {
            try
            {
                var receptionists = await _hospitalService.GetHospitalReceptionists(hospitalId, receptionistName, pageNumber);
                return Ok(receptionists);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("System", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [HttpPost]
        public async Task<IActionResult> AddHospitals(HospitalDto model)
        {
            var validation = _hospitalValidator.Validate(model);
            if (validation.IsValid)
            {
                try
                {
                    await _hospitalService.AddHospital(model);
                    return Ok(new { Message = "New Hospital Added Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("System", ex.Message);
                }

            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut("{hospitalId}")]
        public async Task<IActionResult> UpdateHospital(Guid hospitalId, HospitalDto model)
        {
            var validation = _hospitalValidator.Validate(model);
            if (validation.IsValid)
            {
                try
                {
                    await _hospitalService.UpdateHospital(hospitalId, model);
                    return Ok(new { Message = "Hospital Updated Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("System", ex.Message);
                }

            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpDelete("{hospitalId}")]
        public async Task<IActionResult> DeleteHospital(Guid hospitalId)
        {
            try
            {
                await _hospitalService.DeleteHospital(hospitalId);
                return Ok(new { Message = "Hospital Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("System", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
