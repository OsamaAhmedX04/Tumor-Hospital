using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Response.MRI
{
    public class ExplainResponseDto
    {
        [JsonPropertyName("predicted_class")]
        public string PredictedClass { get; set; }

        [JsonPropertyName("confidence")]
        public decimal Confidence { get; set; }

        [JsonPropertyName("probabilities")]
        public ProbabilitiesDto Probabilities { get; set; }
    }
}
