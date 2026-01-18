using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

namespace TumorHospital.Infrastructure.Services
{
    public class ReceptionService : IReceptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReceptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<PageSourcePagination<ReceptionistDto>> GetAllReceptionists(int PageNumber, string? receptionistName)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: r => ($"{r.User.FirstName} {r.User.LastName}").Contains(receptionistName ?? ""),
                selector: r => new ReceptionistDto
                {
                    Id = r.ApplicationUserId,
                    IsActive = r.User.IsActive,
                    Name = r.User.FirstName + " " + r.User.LastName,
                },
                pageSize: 20,
                pageNumber: PageNumber
                );
        }



        public async Task<ReceptionistDetailsDto> GetReceptionist(string id)
        {
            return await _unitOfWork.Receptionists.GetEnhancedAsync(
                filter: r => r.ApplicationUserId == id,
                selector: r => new ReceptionistDetailsDto
                {
                    Id = r.ApplicationUserId,
                    IsActive = r.User.IsActive,
                    IsDeleted = r.IsDeleted,
                    Name = r.User.FirstName + " " + r.User.LastName,
                    Email = r.User.Email!,
                    Gender = r.Gender
                }
                ) ?? new ReceptionistDetailsDto();
        }


    }
}
