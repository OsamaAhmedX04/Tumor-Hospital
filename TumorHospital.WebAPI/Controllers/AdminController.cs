using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Documentation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Authorize(Roles = SystemRole.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminSevice _adminSevice;
        private readonly IValidator<NewDoctorDto> _doctorValidator;
        private readonly IValidator<NewReceptionistDto> _receptionistValidator;



        public AdminController(
            IAdminSevice adminSevice,
            IValidator<NewDoctorDto> doctorValidator,
            IValidator<NewReceptionistDto> receptionistValidator)
        {
            _adminSevice = adminSevice;
            _doctorValidator = doctorValidator;
            _receptionistValidator = receptionistValidator;
        }

        [SwaggerOperation(Summary = AdminDocs.CreateNewDoctorSummary, Description = AdminDocs.CreateNewDoctorDescription)]
        [HttpPost("create-doctor")]
        public async Task<IActionResult> CreateNewDoctor([FromBody] NewDoctorDto model)
        {
            var validation = _doctorValidator.Validate(model);
            if (validation.IsValid)
            {
                try
                {
                    await _adminSevice.CreateNewDoctor(model);
                    return Ok(new { Message = "New Doctor Account Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AdminDocs.CreateNewReceptionistSummary, Description = AdminDocs.CreateNewReceptionistDescription)]
        [HttpPost("create-receptionist")]
        public async Task<IActionResult> CreateNewReceptionist(NewReceptionistDto model)
        {
            var validation = _receptionistValidator.Validate(model);
            if (validation.IsValid)
            {
                try
                {
                    await _adminSevice.CreateNewReceptionist(model);
                    return Ok(new { Message = "New Receptionist Account Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AdminDocs.DeleteDoctorSummary, Description = AdminDocs.DeleteDoctorDescription)]
        [HttpDelete("Doctor/{doctorId}")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            try
            {
                await _adminSevice.DeleteDoctor(doctorId);
                return Ok(new { Message = "Doctor Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = AdminDocs.DeleteReceptionistSummary, Description = AdminDocs.DeleteReceptionistDescription)]
        [HttpDelete("Receptionist/{receptionistId}")]
        public async Task<IActionResult> DeleteReceptionist(string receptionistId)
        {
            try
            {
                await _adminSevice.DeleteReceptionist(receptionistId);
                return Ok(new { Message = "Receptionist Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AdminDocs.GetDashboardSummary, Description = AdminDocs.GetDashboardDescription)]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(await _adminSevice.GetDashboardDataAsync());
        }
    }
}
