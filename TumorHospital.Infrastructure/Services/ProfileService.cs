using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private static List<string> AllowableExtensions = new List<string> { ".png", ".jpg", ".jpeg" };
        private static int AllowableSize = 1 * 1024 * 1024; // 1MB

        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ProfileService(IFileService fileService, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task UploadProfilePicture(IFormFile file)
        {
            var userId = _currentUserService.UserId;
            var isNullableFile = file is null || file.Length == 0;
            if (isNullableFile) throw new Exception("Please Upload The Image");

            var isValidSize = file.Length <= AllowableSize;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var isValidExtension = AllowableExtensions.Contains(extension);

            if (!isValidExtension)
                throw new Exception($"Invalid Image Extension. Allowable Extensions: {string.Join(',', AllowableExtensions)}");
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

        public async Task<PatientProfileResponse> GetPatientProfile()
        {
            var userId = _currentUserService.UserId;
            var patient = await _unitOfWork.Patients.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (patient == null) throw new Exception("Patient not found");

            return _mapper.Map<PatientProfileResponse>(patient);
        }

        public async Task<bool> UpdateProfile(UpdatePatientProfileDto dto)
        {
            var userId = _currentUserService.UserId;
            var patient = await _unitOfWork.Patients.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (patient == null) return false;

            _mapper.Map(dto, patient);
            _mapper.Map(dto, patient.User);

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<DoctorProfileResponse> GetDoctorProfile()
        {
            var userId = _currentUserService.UserId;
            var doctor = await _unitOfWork.Doctors.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: new Expression<Func<Doctor, object>>[]
                {
                    d => d.User,
                    d => d.Specialization
                }
            );


            if (doctor == null) throw new Exception("Doctor not found");

            return _mapper.Map<DoctorProfileResponse>(doctor);
        }

        public async Task<bool> UpdateProfile(UpdateDoctorProfileDto dto)
        {
            var userId = _currentUserService.UserId;
            var doctor = await _unitOfWork.Doctors.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (doctor == null) return false;

            _mapper.Map(dto, doctor);
            _mapper.Map(dto, doctor.User);

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<ReceptionistProfileResponse> GetReceptionistProfile()
        {
            var userId = _currentUserService.UserId;
            var receptionist = await _unitOfWork.Receptionists.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (receptionist == null) throw new Exception("Receptionist not found");

            return _mapper.Map<ReceptionistProfileResponse>(receptionist);
        }

        public async Task<bool> UpdateProfile(UpdateReceptionistProfileDto dto)
        {
            var userId = _currentUserService.UserId;
            var receptionist = await _unitOfWork.Receptionists.GetAsync(
                selector: x => x,
                filter: x => x.ApplicationUserId == userId,
                Includes: x => x.User
            );

            if (receptionist == null) return false;

            _mapper.Map(dto, receptionist);
            _mapper.Map(dto, receptionist.User);

            _unitOfWork.Receptionists.Update(receptionist);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
