using TumorHospital.Application.DTOs.Request.Hospital;
using TumorHospital.Application.DTOs.Response.Hospital;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IHospitalService
    {
        Task AddHospital(HospitalDto model);
        Task UpdateHospital(Guid id, HospitalDto model);
        Task DeleteHospital(Guid id);

        Task<List<HospitalInfoDto>> GetHospitals();
        Task<HospitalDashboardDto> GetHospitalDashboard(Guid id);
        Task<PageSourcePagination<DoctorDto>> GetHospitalDoctors(Guid id, int pageNumber = 1, string? doctorName = null, string? specializationName = null);
        Task<DoctorInformationDto> GetHospitalDoctor(string doctorId);
        Task<PageSourcePagination<ReceptionistDto>> GetHospitalReceptionists(Guid id, string? receptionistName = null, int pageNumber = 1);
        Task<List<string>> GetHospitalGovernments();
        Task<List<string>> GetHospitalsNames();
    }
}
