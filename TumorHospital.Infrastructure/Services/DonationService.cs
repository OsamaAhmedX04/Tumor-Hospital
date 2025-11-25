using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

namespace TumorHospital.Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task AddNeed(NeedDto need)
        {
            throw new NotImplementedException();
        }

        public Task AddNeed(NeedDetailsDto need)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNeed(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Donate(VolunteerDto volunteer)
        {
            throw new NotImplementedException();
        }

        public Task<PageSourcePagination<NeedDto>> GetAllNeeds(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<PageSourcePagination<VolunteerDto>> GetAllVolunteers(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public string GetCategoriesOfNeeds()
        {
            throw new NotImplementedException();
        }

        public Task<NeedDetailsDto> GetNeed(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNeed(UpdateNeedDto newNeed, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
