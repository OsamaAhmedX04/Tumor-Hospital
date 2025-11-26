using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAdmins(int pageSize, int pageNumber) 
            => Ok(await _userService.GetAllAdmins(pageSize, pageNumber));

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatientss(int pageSize, int pageNumber)
            => Ok(await _userService.GetAllPatients(pageSize, pageNumber));

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors(int pageSize, int pageNumber)
            => Ok(await _userService.GetAllDoctors(pageSize, pageNumber));

        [HttpGet("receptionists")]
        public async Task<IActionResult> GetReceptionists(int pageSize, int pageNumber)
            => Ok(await _userService.GetAllReceptionist(pageSize, pageNumber));

        [HttpGet("inactive-doctors")]
        public async Task<IActionResult> GetInActiveDoctors(int pageSize, int pageNumber)
            => Ok(await _userService.GetAllInActiveDoctorRoles(pageSize, pageNumber));

        [HttpGet("inactive-receptionists")]
        public async Task<IActionResult> GetInActiveReceptionists(int pageSize, int pageNumber)
            => Ok(await _userService.GetAllInActiveReceptionistRoles(pageSize, pageNumber));
    }
}
