using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.Admin;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAdminSevice
    {
        Task CreateNewDoctor(NewDoctorDto model);
        Task DeleteDoctor(string doctorId);
        Task CreateNewReceptionist(NewReceptionistDto model);
        Task DeleteReceptionist(string receptionistId);
        Task<AdminDashboardResponse> GetDashboardDataAsync();
    }
}
