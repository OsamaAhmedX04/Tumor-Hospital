using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

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

        public async Task<PatientProfileResponse> GetPatientProfile(string userId)
        {
            var patient = await _unitOfWork.Patients.GetEnhancedAsync(
                selector: x => new PatientProfileResponse
                {
                    ApplicationUserId = x.ApplicationUserId,
                    FullName = $"{x.User.FirstName} {x.User.LastName}",
                    Email = x.User.Email,
                    Gender = x.Gender,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth
                },
                filter: x => x.ApplicationUserId == userId
            );

            return patient?? throw new Exception ("Patient not found");
        }

        public async Task<bool> UpdateProfile(string userId, UpdatePatientProfileDto dto)
        {
            var patient = await _unitOfWork.Patients.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (patient == null) return false;

            patient.User.FirstName = dto.FirstName;
            patient.User.LastName = dto.LastName;
            patient.User.PhoneNumber = dto.PhoneNumber;
            patient.Gender = dto.Gender;
            patient.Address = dto.Address;
            patient.DateOfBirth = dto.DateOfBirth;

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<DoctorProfileResponse> GetDoctorProfile(string userId)
        {
            var doctor = await _unitOfWork.Doctors.GetEnhancedAsync(
                selector: x => new DoctorProfileResponse
                {
                    ApplicationUserId = x.ApplicationUserId,
                    FullName = $"{x.User.FirstName} {x.User.LastName}",
                    Email = x.User.Email,
                    Gender = x.Gender,
                    Bio = x.Bio,
                    ProfilePicturePath = SupabaseConstants.PrefixSupaURL + x.ProfilePicturePath,
                    SpecializationName = x.Specialization.Name
                },
                filter: x => x.ApplicationUserId == userId
            );

            return doctor ?? throw new Exception("Doctor not found");
        }

        public async Task<bool> UpdateProfile(string userId, UpdateDoctorProfileDto dto)
        {
            var doctor = await _unitOfWork.Doctors.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (doctor == null)
                return false;

            doctor.User.FirstName = dto.FirstName;
            doctor.User.LastName = dto.LastName;
            doctor.User.PhoneNumber = dto.PhoneNumber;
            doctor.Gender = dto.Gender;
            doctor.Bio = dto.Bio;

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
