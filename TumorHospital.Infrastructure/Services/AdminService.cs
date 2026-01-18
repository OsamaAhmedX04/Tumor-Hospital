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

        public AdminService(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
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

            var hospital = await _unitOfWork.Hospitals.FirstOrDefaultAsync(h => h.Name == model.HospitalName);
            if (hospital is null)
                throw new Exception("This Hospital Not Exist");
            if (hospital.Doctors.Count() == hospital.MaxNumberOfDoctors)
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


            //var body = $@"
            //    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
            //        <h2 style='text-align: center; color: #333;'>Welcome to Our Hospital!</h2>

            //        <p style='font-size: 15px; color: #555;'>
            //            Dear Dr. {appUser.FirstName} {appUser.LastName},<br/><br/>
            //            Your account has been successfully created. Below is your temporary password to access your account:
            //        </p>

            //        <div style='text-align: center; margin: 30px 0;'>
            //            <span style='display: inline-block; background-color: #28a745; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 2px; border-radius: 6px;'>
            //                {doctorPassword}
            //            </span>
            //        </div>

            //        <p style='font-size: 14px; color: #666;'>
            //            ⚠️ For security reasons, please <strong>do not share this password with anyone</strong>.<br/>
            //            You are required to change it after your first login.
            //        </p>

            //        <p style='margin-top: 30px; font-size: 14px; color: #333;'>
            //            Best regards,<br/>
            //            <strong>The Hospital Admin Team</strong>
            //        </p>
            //    </div>
            //    ";

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

            await _userManager.DeleteAsync(doctor);
        }

        public async Task CreateNewReceptionist(NewReceptionistDto model)
        {

            var hospital = await _unitOfWork.Hospitals.FirstOrDefaultAsync(h => h.Name == model.HospitalName);
            if (hospital is null)
                throw new Exception("This Hospital Not Exist");
            if (hospital.Receptionists.Count() == hospital.MaxNumberOfReceptionists)
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

            //var body = $@"
            //    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
            //        <h2 style='text-align: center; color: #333;'>Welcome to Our Hospital Team!</h2>

            //        <p style='font-size: 15px; color: #555;'>
            //            Dear {appUser.FirstName} {appUser.LastName},<br/><br/>
            //            We’re pleased to inform you that your <strong>Receptionist account</strong> has been successfully created.<br/>
            //            You can now log in to the system using the temporary password below:
            //        </p>

            //        <div style='text-align: center; margin: 30px 0;'>
            //            <span style='display: inline-block; background-color: #007bff; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 2px; border-radius: 6px;'>
            //                {receptionistPassword}
            //            </span>
            //        </div>

            //        <p style='font-size: 14px; color: #666;'>
            //            ⚠️ For security reasons, please <strong>do not share this password with anyone</strong>.<br/>
            //            You are required to change it after your first login.
            //        </p>

            //        <p style='margin-top: 30px; font-size: 14px; color: #333;'>
            //            Best regards,<br/>
            //            <strong>The Hospital Admin Team</strong>
            //        </p>
            //    </div>
            //";

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
