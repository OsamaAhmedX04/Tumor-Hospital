using TumorHospital.Application.DTOs.Request.ML;
using TumorHospital.Application.DTOs.Response.ML;

namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface IMLService
    {
        Task<ChatResponseDto> ChatAsync(ChatRequestDto request);

        Task ExplainAsync(ExplainRequestDto dto);

        Task<DiagnosticResponseDto> GetDiagnosticByAppointmentIdAsync(Guid appointmentId);
    }
}
