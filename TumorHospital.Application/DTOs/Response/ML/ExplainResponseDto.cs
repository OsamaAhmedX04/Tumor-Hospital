using System.Text.Json.Serialization;

namespace TumorHospital.Application.DTOs.Response.ML
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
