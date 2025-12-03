using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

namespace TumorHospital.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private static List<string> AllowableExtensions = new List<string> {".png", ".jpg", ".jpeg" };
        private static int AllowableSize = 1 * 1024 * 1024; // 1MB

        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task UploadProfilePicture(IFormFile file, string userId)
        {
            var isNullableFile = file is null || file.Length == 0;
            if (isNullableFile) throw new Exception("Please Upload The Image");

            var isValidSize = file.Length <= AllowableSize;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var isValidExtension = AllowableExtensions.Contains(extension);

            if (!isValidExtension) 
                throw new Exception($"Invalid Image Extension. Allowable Extensions: {string.Join(',',AllowableExtensions)}");
            if (!isValidSize) 
                throw new Exception("Size Of Image Must Not Exceed 1MB");

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(userId);
            if (doctor is null) throw new Exception("This Doctor Does Not Exist");

            string filePath = string.Empty;
            if (doctor.ProfilePicturePath != null)
                filePath = await _fileService.EditAsync(doctor.ProfilePicturePath, file);
            else
                filePath = await _fileService.UploadAsync(file, "Images/Doctors");
            
            doctor.ProfilePicturePath = filePath;
            await _unitOfWork.CompleteAsync();
        }
    }
}
