using AutoMapper;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public DonationService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task AddNeed(NewNeedDto need)
        {
            var needToAdd = _mapper.Map<CharityNeed>(need);

            needToAdd.ImagePath = await _fileService.UploadAsync(need.Image, "Images/CharityNeeds");

            await _unitOfWork.CharityNeeds.AddAsync(needToAdd);

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteNeed(Guid id)
        {
            var isExist = await _unitOfWork.CharityNeeds.IsExistAsync(id);
            if (!isExist)
                throw new Exception("Need not found");

            _unitOfWork.CharityNeeds.Delete(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Donate(VolunteerDto volunteer)
        {
            var need = await _unitOfWork.CharityNeeds.GetByIdAsync(volunteer.CharityNeedId);

            if (need is null)
                throw new Exception("Need not found");

            var volunteerToAdd = _mapper.Map<VolunteerDonation>(volunteer);
            await _unitOfWork.VolunteerDonations.AddAsync(volunteerToAdd);
            need.CollectedAmount += volunteer.AmountDonated;

            await _unitOfWork.CompleteAsync();
        }

        public async Task<PageSourcePagination<NeedDto>> GetAllNeeds(int pageSize, int pageNumber)
        {
            return await _unitOfWork.CharityNeeds.GetAllPaginatedEnhancedAsync(
                selector: need => new NeedDto
                {
                    Title = need.Title,
                    ImagePath = SupabaseConstants.PrefixSupaURL + need.ImagePath,
                    CharityCategory = need.Category.ToString(),
                    CreatedAt = need.CreatedAt
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<NeedDetailsDto> GetNeed(Guid id)
        {
            var isExist = await _unitOfWork.CharityNeeds.IsExistAsync(id);
            if (!isExist)
                throw new Exception("Need not found");

            return await _unitOfWork.CharityNeeds.GetEnhancedAsync(
                filter: need => need.Id == id,
                selector: need => new NeedDetailsDto
                {
                    Title = need.Title,
                    Description = need.Description,
                    ImagePath = SupabaseConstants.PrefixSupaURL + need.ImagePath,
                    CharityCategory = need.Category.ToString(),
                    NeedAmount = need.NeedAmount,
                    CollectedAmount = need.CollectedAmount,
                    IsCompleted = need.IsCompleted,
                    CreatedAt = need.CreatedAt
                }
                ) ?? throw new Exception("Need not found");
        }


        public CharityCategoriesDto GetCategoriesOfNeeds()
        {
            var categories = Enum.GetNames(typeof(CharityCategory)).ToList();
            var dto = new CharityCategoriesDto
            {
                Categories = categories
            };
            return dto;
        }


        public async Task UpdateNeed(UpdateNeedDto newNeed, Guid id)
        {
            var need = await _unitOfWork.CharityNeeds.GetByIdAsync(id);
            if (need is null)
                throw new Exception("Need not found");

            need.Title = newNeed.Title;
            need.Description = newNeed.Description;
            need.ImagePath = await _fileService.EditAsync(need.ImagePath, newNeed.Image);
            need.NeedAmount = newNeed.NeedAmount;
            need.Category = Enum.Parse<CharityCategory>(newNeed.CharityCategory);

            await _unitOfWork.CompleteAsync();
        }


        public async Task<PageSourcePagination<VolunteerInfoDto>> GetAllVolunteers(int pageSize, int pageNumber)
        {
            return await _unitOfWork.VolunteerDonations.GetAllPaginatedEnhancedAsync(
                selector: volunteer => new VolunteerInfoDto
                {
                    VolunteerName = volunteer.VolunteerName,
                    Email = volunteer.Email,
                    Phone = volunteer.Phone,
                    AmountDonated = volunteer.AmountDonated,
                    CharityNeedCategory = volunteer.CharityNeed!.Category.ToString(),
                    DonationDate = volunteer.DonationDate
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

    }
}
