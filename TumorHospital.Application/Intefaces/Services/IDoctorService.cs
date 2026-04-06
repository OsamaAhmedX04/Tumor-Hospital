using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IDoctorService
    {
        Task<PageSourcePagination<DoctorDto>> GetDoctors(int pageNumber, string? workDay = null, bool? IsVideoCallDoctor = null, string? government = null, string? specializationName = null);
        Task<DoctorDetailsDto> GetDoctorDetails(string doctorId, string patientId);
    }
}
