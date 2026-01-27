using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Domain.Constants;
using TumorHospital.WebAPI.Documentation;

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

        [SwaggerOperation(Summary = VolunteerDocs.GetAllVolunteersSummary, Description = VolunteerDocs.GetAllVolunteersDescription)]
        //[Authorize(Roles = SystemRole.Admin + "," + SystemRole.Receptionist)]
        [HttpGet("Volunteers")]
        public async Task<IActionResult> GetAllVolunteers(int pageNumber)
        {
            var volunteers = await _volunteerService.GetAllVolunteers(pageNumber);
            return Ok(volunteers);
        }
    }
}
