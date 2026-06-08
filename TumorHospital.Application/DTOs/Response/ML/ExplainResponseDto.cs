using System.Text.Json.Serialization;

namespace TumorHospital.Application.DTOs.Response.ML
{
    public class ExplainResponseDto
    {
        [JsonPropertyName("predicted_class")]
        public string PredictedClass { get; set; }

        [JsonPropertyName("confidence")]
        public decimal Confidence { get; set; }

        [JsonPropertyName("confidence_band")]
        public string ConfidenceBand { get; set; }

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }

        [JsonPropertyName("override_applied")]
        public bool OverrideApplied { get; set; }

        [JsonPropertyName("prediction")]
        public PredictionDto Prediction { get; set; }
    }
}
