namespace TumorHospital.Application.DTOs.Response.ML
{
    public class PredictionDto
    {
        public decimal Confidence { get; set; }
        public string PredictedClass { get; set; }
        public ProbabilitiesDto Probabilities { get; set; }
    }
}
