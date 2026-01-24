using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

namespace TumorHospital.Infrastructure.Services
{
    public class VolunteerService : IVolunteerService
    {

        private readonly IUnitOfWork _unitOfWork;

        public VolunteerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
