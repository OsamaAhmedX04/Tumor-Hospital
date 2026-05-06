using Microsoft.AspNetCore.Http;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IProfileService
    {
        Task UploadProfilePicture(IFormFile file);
        Task<PatientProfileResponse> GetPatientProfile();
        Task<bool> UpdateProfile(UpdatePatientProfileDto dto);
        Task<DoctorProfileResponse> GetDoctorProfile();
        Task<bool> UpdateProfile(UpdateDoctorProfileDto dto);
        Task<ReceptionistProfileResponse> GetReceptionistProfile();
        Task<bool> UpdateProfile(UpdateReceptionistProfileDto dto);
    }
}
