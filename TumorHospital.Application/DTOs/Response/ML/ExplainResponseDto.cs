namespace TumorHospital.Application.DTOs.Response.ML
{
    public class ExplainResponseDto
    {
        public string PredictedClass { get; set; }
        public decimal Confidence { get; set; }
        public string ConfidenceBand { get; set; }
        public string Explanation { get; set; }
        public bool OverrideApplied { get; set; }
        public PredictionDto Prediction { get; set; }
    }
}
