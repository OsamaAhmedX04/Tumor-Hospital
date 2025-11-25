using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.Intefaces.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNeeds()
        {
            throw new NotImplementedException();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNeed([FromQuery] Guid id)
        {
            throw new NotImplementedException();
        }
        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesOfNeeds()
        {
            throw new NotImplementedException();
        }
        [HttpGet("Volunteers")]
        public async Task<IActionResult> GetAllVolunteers()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddNeed()
        {
            throw new NotImplementedException();
        }
        [HttpPost("Donate")]
        public async Task<IActionResult> Donate()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNeed()
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNeed(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
