using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Response.ML;

namespace TumorHospital.Application.DTOs.Response.MRI
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
