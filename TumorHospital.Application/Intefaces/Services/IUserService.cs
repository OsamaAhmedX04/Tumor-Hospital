using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IUserService
    {
        Task<PageSourcePagination<UserWithIdDto>> GetAllDoctors(int pageNumber);
        Task<PageSourcePagination<UserWithIdDto>> GetAllPatients(int pageNumber);
        Task<PageSourcePagination<UserWithIdDto>> GetAllAdmins(int pageNumber);
        Task<PageSourcePagination<UserWithIdDto>> GetAllReceptionist(int pageNumber);
        Task<PageSourcePagination<UserWithIdDto>> GetAllInActiveDoctorRoles(int pageNumber);
        Task<PageSourcePagination<UserWithIdDto>> GetAllInActiveReceptionistRoles(int pageNumber);
    }
}
