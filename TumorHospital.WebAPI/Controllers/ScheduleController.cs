using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IValidator<DoctorScheduleDto> _scheduleValidator;

        public ScheduleController(IScheduleService scheduleService, IValidator<DoctorScheduleDto> scheduleValidator)
        {
            _scheduleService = scheduleService;
            _scheduleValidator = scheduleValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorSchedule(string doctorId)
            =>Ok(await _scheduleService.GetDoctorSchedule(doctorId));
        

        [HttpPost]
        public async Task<IActionResult> AddSchedule(string doctorId, DoctorScheduleDto schedule)
        {
            var validationResult = _scheduleValidator.Validate(schedule);
            if (validationResult.IsValid)
            {
                try
                {
                    await _scheduleService.AddSchedule(doctorId, schedule);
                    return Ok(new { Message = "Schedule Created Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchedule(Guid scheduleId, string doctorId, DoctorScheduleDto schedule)
        {
            var validationResult = _scheduleValidator.Validate(schedule);
            if (validationResult.IsValid)
            {
                try
                {
                    await _scheduleService.UpdateScheduale(scheduleId, doctorId, schedule);
                    return Ok(new { Message = "Schedule Updated Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Message", ex.Message);
                }
            }
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSchedule(Guid scheduleId)
        {
            try
            {
                await _scheduleService.DeleteScheduale(scheduleId);
                return Ok(new { Message = "Schedule Deleted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Message", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }
    }
}
