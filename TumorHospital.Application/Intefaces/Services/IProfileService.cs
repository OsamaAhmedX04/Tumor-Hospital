using Microsoft.AspNetCore.Http;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IProfileService
    {
        Task UploadProfilePicture(IFormFile file, string userId);
        Task<PatientProfileResponse> GetPatientProfile(string userId);
        Task<bool> UpdateProfile(string userId, UpdatePatientProfileDto dto);
        Task<DoctorProfileResponse> GetDoctorProfile(string userId);
        Task<bool> UpdateProfile(string userId, UpdateDoctorProfileDto dto);
        Task<ReceptionistProfileResponse> GetReceptionistProfile(string userId);
        Task<bool> UpdateProfile(string userId, UpdateReceptionistProfileDto dto);
    }
}
