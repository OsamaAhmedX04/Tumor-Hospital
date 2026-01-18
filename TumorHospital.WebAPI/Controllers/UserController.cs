using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins(int pageNumber)
            => Ok(await _userService.GetAllAdmins(pageNumber));

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatientss(int pageNumber)
            => Ok(await _userService.GetAllPatients(pageNumber));

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors(int pageNumber)
            => Ok(await _userService.GetAllDoctors(pageNumber));

        [HttpGet("receptionists")]
        public async Task<IActionResult> GetReceptionists(int pageNumber)
            => Ok(await _userService.GetAllReceptionist(pageNumber));

        [HttpGet("inactive-doctors")]
        public async Task<IActionResult> GetInActiveDoctors(int pageNumber)
            => Ok(await _userService.GetAllInActiveDoctorRoles(pageNumber));

        [HttpGet("inactive-receptionists")]
        public async Task<IActionResult> GetInActiveReceptionists(int pageNumber)
            => Ok(await _userService.GetAllInActiveReceptionistRoles(pageNumber));
    }
}
