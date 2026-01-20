using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.Helpers;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class AdminService : IAdminSevice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager, IEmailService emailService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _fileService = fileService;
        }

        public async Task CreateNewDoctor(NewDoctorDto model)
        {
            var specialization = await _unitOfWork.Specializations
                .GetEnhancedAsync(
                filter: s => s.Name == model.SpecializationName,
                selector: s => new { s.Id }
                );
            if (specialization is null)
                throw new Exception("This Specialization Is Not Exist");

            var hospital = await _unitOfWork.Hospitals.GetEnhancedAsync(
                filter: h => h.Id == model.HospitalId,
                selector: h => new
                {
                    Id = h.Id,
                    MaxNumberOfDoctors = h.MaxNumberOfDoctors,
                    NumberOfDoctors = h.Doctors.Count(),
                }
                );
            if (hospital is null)
                throw new Exception("This Hospital Not Exist");
            if (hospital.NumberOfDoctors == hospital.MaxNumberOfDoctors)
                throw new Exception("This Hospital has Reached the max number of doctors");

            var appUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                EmailConfirmed = true,
                Email = model.Email,
            };
            string doctorPassword = Generator.GenerateRandomPassword();
            var result = await _userManager.CreateAsync(appUser, doctorPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var createdUser = await _userManager.FindByEmailAsync(model.Email);
            await _userManager.AddToRoleAsync(createdUser, Role.InActiveDoctorRole.ToString());


            var doctor = _mapper.Map<Doctor>(model);
            doctor.ApplicationUserId = createdUser.Id;
            doctor.SpecializationId = specialization.Id;
            doctor.HospitalId = hospital.Id;

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(
                appUser.Email,
                "Doctor Account Activated",
                EmailBody.GetDoctorEmailCreatedBody(appUser.FirstName, appUser.LastName, doctorPassword));

        }

        public async Task DeleteDoctor(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null)
                throw new Exception("This User Not Exist");

            string? doctorImagePath = await _unitOfWork.Doctors.GetEnhancedAsync
                (
                filter: d => d.ApplicationUserId == doctorId,
                selector: d => d.ProfilePicturePath ?? "N/A"
                );

            if (doctorImagePath == "N/A")
                await _fileService.DeleteAsync(SupabaseConstants.PrefixSupaURL + doctorImagePath);

            await _userManager.DeleteAsync(doctor);
        }

        public async Task CreateNewReceptionist(NewReceptionistDto model)
        {

            var hospital = await _unitOfWork.Hospitals.GetEnhancedAsync(
                filter: h => h.Id == model.HospitalId,
                selector: h => new
                {
                    Id = h.Id,
                    MaxNumberOfReceptionists = h.MaxNumberOfReceptionists,
                    NumberOfReceptionists = h.Receptionists.Count(),
                }
                );
            if (hospital is null)
                throw new Exception("This Hospital Not Exist");
            if (hospital.NumberOfReceptionists == hospital.MaxNumberOfReceptionists)
                throw new Exception("This Hospital has Reached the max number of receptionists");

            var appUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                EmailConfirmed = true,
                Email = model.Email,
            };
            string receptionistPassword = Generator.GenerateRandomPassword();
            var result = await _userManager.CreateAsync(appUser, receptionistPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var createdUser = await _userManager.FindByEmailAsync(model.Email);
            await _userManager.AddToRoleAsync(createdUser, Role.InActiveReceptionistRole.ToString());


            var receptionist = _mapper.Map<Receptionist>(model);
            receptionist.ApplicationUserId = createdUser.Id;
            receptionist.HospitalId = hospital.Id;

            await _unitOfWork.Receptionists.AddAsync(receptionist);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(
                appUser.Email,
                "Receptionist Account Activated",
                EmailBody.GetReceptionistEmailCreatedBody(appUser.FirstName, appUser.LastName, receptionistPassword));

        }

        public async Task DeleteReceptionist(string receptionistId)
        {
            var receptionist = await _unitOfWork.Receptionists.GetByIdAsync(receptionistId);
            if (receptionist == null)
                throw new Exception("This User Not Exist");

            receptionist.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }




    }
}
