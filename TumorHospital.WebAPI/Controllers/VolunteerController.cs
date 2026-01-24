using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _volunteerService;
        public VolunteerController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }


        [HttpGet("Volunteers")]
        public async Task<IActionResult> GetAllVolunteers(int pageSize, int pageNumber)
        {
            var volunteers = await _volunteerService.GetAllVolunteers(pageSize, pageNumber);
            return Ok(volunteers);
        }
    }
}
