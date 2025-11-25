using Microsoft.AspNetCore.Http;

namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string? folder = null);
        Task<string> EditAsync(string existingFilePath, IFormFile newFile);
        Task DeleteAsync(string filePath);
    }
}
