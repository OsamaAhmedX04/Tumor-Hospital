namespace TumorHospital.Application.DTOs.Request.Pharmacy
{
    public class NewPharmacyDto
    {
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
