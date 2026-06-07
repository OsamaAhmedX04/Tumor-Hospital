namespace TumorHospital.Application.DTOs.Response.ML
{
    public class ProbabilitiesDto
    {
        public decimal Glioma { get; set; }
        public decimal Meningioma { get; set; }
        public decimal Notumor { get; set; }
        public decimal Pituitary { get; set; }
    }
}
