namespace TumorHospital.Application.DTOs.Response.ML
{
    public class MlApiErrorDto
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public bool RetrySuggested { get; set; }
    }
}
