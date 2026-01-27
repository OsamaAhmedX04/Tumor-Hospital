using AutoMapper;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DonationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Donate(VolunteerDto volunteer)
        {
            var need = await _unitOfWork.CharityNeeds.GetByIdAsync(volunteer.CharityNeedId);

            if (need is null)
                throw new Exception("Need not found");

            var volunteerToAdd = _mapper.Map<VolunteerDonation>(volunteer);
            await _unitOfWork.VolunteerDonations.AddAsync(volunteerToAdd);

            need.CollectedAmount += volunteer.AmountDonated;
            if (need.NeedAmount <= volunteer.AmountDonated) need.IsCompleted = true;

            await _unitOfWork.CompleteAsync();
        }
    }
}
