using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.Hospital;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Documentation;
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

        [SwaggerOperation(Summary = HospitalDocs.GetAllHospitalsSummary, Description = HospitalDocs.GetAllHospitalsDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
        [HttpGet("/api/Hospitals")]
        public async Task<IActionResult> GetAllHospitals()
        {
            return Ok(await _hospitalService.GetHospitals());
        }




        [SwaggerOperation(Summary = HospitalDocs.GetHospitalDashboardSummary, Description = HospitalDocs.GetHospitalDashboardDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
        [HttpGet("dashboard/{hospitalId}")]
        public async Task<IActionResult> GetHospitalDashboard(Guid hospitalId)
        {
            try
            {
                var dashboard = await _hospitalService.GetHospitalDashboard(hospitalId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("NotFound", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });

        }




        [SwaggerOperation(Summary = HospitalDocs.GetAllHospitalDoctorsSummary, Description = HospitalDocs.GetAllHospitalDoctorsDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
        [HttpGet("{hospitalId}/doctors")]
        public async Task<IActionResult> GetAllHospitalDoctors(Guid hospitalId, int pageNumber, string? doctorName = null, string? specializationName = null)
        {
            try
            {
                var doctors = await _hospitalService.GetHospitalDoctors(hospitalId, pageNumber, doctorName, specializationName);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("System", ex.Message);
            }

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }





        [SwaggerOperation(Summary = HospitalDocs.GetHospitalDoctorSummary, Description = HospitalDocs.GetHospitalDoctorDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetHospitalDoctor(string doctorId)
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




        [SwaggerOperation(Summary = HospitalDocs.GetAllHospitalReceptionistsSummary, Description = HospitalDocs.GetAllHospitalReceptionistsDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
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




        [SwaggerOperation(Summary = HospitalDocs.GetHospitalGovernmentsExistanceSummary, Description = HospitalDocs.GetHospitalGovernmentsExistanceDescription)]
        [HttpGet("governments")]
        public async Task<IActionResult> GetHospitalGovernmentsExistance()
            => Ok(await _hospitalService.GetHospitalGovernments());



        [SwaggerOperation(Summary = HospitalDocs.GetHospitalsNamesSummary, Description = HospitalDocs.GetHospitalsNamesDescription)]
        [HttpGet("hospital-names")]
        public async Task<IActionResult> GetHospitalsNames()
            => Ok(await _hospitalService.GetHospitalsNames());




        [SwaggerOperation(Summary = HospitalDocs.AddHospitalSummary, Description = HospitalDocs.AddHospitalDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddHospital(HospitalDto model)
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




        [SwaggerOperation(Summary = HospitalDocs.UpdateHospitalSummary, Description = HospitalDocs.UpdateHospitalDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
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




        [SwaggerOperation(Summary = HospitalDocs.DeleteHospitalSummary, Description = HospitalDocs.DeleteHospitalDescription)]
        //[Authorize(Roles = SystemRole.Admin)]
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
