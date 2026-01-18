namespace TumorHospital.Application.DTOs.Response.Donation
{
    public class VolunteerInfoDto
    {
        public string VolunteerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal AmountDonated { get; set; }
        public string CharityNeedCategory { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
