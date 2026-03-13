using Microsoft.AspNetCore.Http;

namespace TumorHospital.Infrastructure.Services
{
    public interface ITestService
    {
        Task UploadFile(IFormFile file);
        Task<List<string>> GetAllFiles();
    }
}
