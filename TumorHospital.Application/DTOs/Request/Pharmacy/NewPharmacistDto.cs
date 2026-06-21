namespace TumorHospital.Application.DTOs.Request.Pharmacy
{
    public class NewPharmacistDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid PharmacyId { get; set; }
    }
}
