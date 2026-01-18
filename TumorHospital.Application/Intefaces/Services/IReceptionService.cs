using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IReceptionService
    {
        Task<PageSourcePagination<ReceptionistDto>> GetAllReceptionists(int PageNumber, string? receptionistName);

        Task<ReceptionistDetailsDto> GetReceptionist(string id);


    }
}
