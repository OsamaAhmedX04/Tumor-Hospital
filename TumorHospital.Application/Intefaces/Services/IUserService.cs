using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IUserService
    {
        Task<PageSourcePagination<UserDto>> GetAllDoctors(int pageSize, int pageNumber);
        Task<PageSourcePagination<UserDto>> GetAllPatients(int pageSize, int pageNumber);
        Task<PageSourcePagination<UserDto>> GetAllAdmins(int pageSize, int pageNumber);
        Task<PageSourcePagination<UserDto>> GetAllReceptionist(int pageSize, int pageNumber);
        Task<PageSourcePagination<UserDto>> GetAllInActiveRoles(int pageSize, int pageNumber);
    }
}
