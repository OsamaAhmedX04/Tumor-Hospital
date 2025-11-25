using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            throw new NotImplementedException();
        }
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatientss()
        {
            throw new NotImplementedException();

        }
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            throw new NotImplementedException();

        }
        [HttpGet("receptionists")]
        public async Task<IActionResult> GetReceptionists()
        {
            throw new NotImplementedException();

        }
    }
}
