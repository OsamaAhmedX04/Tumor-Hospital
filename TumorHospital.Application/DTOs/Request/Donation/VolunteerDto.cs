namespace TumorHospital.Application.DTOs.Request.Donation
{
    public class VolunteerDto
    {
        public string VolunteerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal AmountDonated { get; set; }
        public Guid? CharityNeedId { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
