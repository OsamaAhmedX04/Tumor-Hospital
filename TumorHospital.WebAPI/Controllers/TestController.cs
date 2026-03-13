using Microsoft.AspNetCore.Mvc;
using TumorHospital.Infrastructure.Services;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = await _testService.GetAllFiles();
            return Ok(files);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await _testService.UploadFile(file);
            return Ok(new { Message = "File Uploaded Successfully MR.AbdelRahman" });
        }
    }
}
