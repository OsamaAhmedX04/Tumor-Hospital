using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto> GetByAppointmentIdAsync(Guid appointmentId);
        Task CreateAsync(Guid appointmentId, PrescriptionCreateDto dto);
        Task UpdateAsync(Guid id, PrescriptionUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
