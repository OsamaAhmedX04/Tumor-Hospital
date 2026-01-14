using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto> CreateAsync(PrescriptionCreateUpdateDto dto);
        Task<PrescriptionResponseDto> GetByAppointmentIdAsync(Guid appointmentId);
        Task<bool> UpdateAsync(Guid id, PrescriptionCreateUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
