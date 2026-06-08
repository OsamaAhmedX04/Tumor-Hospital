using Microsoft.AspNetCore.Http;

namespace TumorHospital.Application.DTOs.Request.ML
{
    public class ExplainRequestDto
    {
        public Guid AppointmentId { get; set; }
        public IFormFile Image { get; set; }
    }
}
