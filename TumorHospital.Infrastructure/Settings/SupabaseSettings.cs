namespace TumorHospital.Infrastructure.Settings
{
    public class SupabaseSettings
    {
        public string Url { get; set; } = null!;
        public string ServiceKey { get; set; } = null!;
        public string BucketName { get; set; } = "TumorHospitalFiles";
    }

}
