namespace TumorHospital.Application.DTOs.Response.ML
{
    public class DiagnosticResponseDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }

        public string ImageURL { get; set; }

        public ExplainResponseDto ExplainResponseDto { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
