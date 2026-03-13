using Microsoft.AspNetCore.Http;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class TestService : ITestService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public TestService(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> GetAllFiles()
        {
            var files = await _unitOfWork.TestFiles.GetAllAsyncEnhanced(x => SupabaseConstants.PrefixSupaURL + x.ImageURL);
            return files;
        }

        public async Task UploadFile(IFormFile file)
        {
            var url = await _fileService.UploadAsync(file, "Test");
            await _unitOfWork.TestFiles.AddAsync(new TestFile { ImageURL = url });
            await _unitOfWork.CompleteAsync();
        }
    }
}
