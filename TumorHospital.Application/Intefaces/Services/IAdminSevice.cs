using TumorHospital.Application.DTOs.Request.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAdminSevice
    {
        Task CreateNewDoctor(NewDoctorDto model);
        Task DeleteDoctor(string doctorId);
        Task CreateNewReceptionist(NewReceptionistDto model);
        Task DeleteReceptionist(string receptionistId);
    }
}
