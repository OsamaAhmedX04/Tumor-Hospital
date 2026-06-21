using Microsoft.AspNetCore.Identity;
using TumorHospital.Application.DTOs.Request.Pharmacy;
using TumorHospital.Application.Helpers;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class PharmacistService : IPharmacistService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public PharmacistService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task CreatePharmacist(NewPharmacistDto dto)
        {
            var isPharmacyExist = await _unitOfWork.Pharmacies.IsExistAsync(dto.PharmacyId);
            if (!isPharmacyExist) throw new Exception("Pharmacy Not Exist");

            var newAppUser = new ApplicationUser
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.Email
            };
            string pharmacistPassword = Generator.GenerateRandomPassword();

            var creationResult = await _userManager.CreateAsync(newAppUser, pharmacistPassword);
            if (!creationResult.Succeeded)
                throw new Exception(string.Join(", ", creationResult.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newAppUser, Role.InActivePharmacist.ToString());

            var newPharmacist = new Pharmacist
            {
                ApplicationUserId = newAppUser.Id,
                PharmacyId = dto.PharmacyId
            };

            await _unitOfWork.Pharmacists.AddAsync(newPharmacist);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(
                newAppUser.Email,
                "Pharmacist Account Activated",
                EmailBody.GetPharmacistEmailCreatedBody(newAppUser.FirstName, newAppUser.LastName, pharmacistPassword));
        }

        public async Task DeletePharmacist(string pharmacistId)
        {
            var pharmacist = await _userManager.FindByIdAsync(pharmacistId);
            if (pharmacist is null) throw new Exception("Pharmacist Not Exist");

            pharmacist.IsDeleted = true;

            await _userManager.UpdateAsync(pharmacist);
        }
    }
}
