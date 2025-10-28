using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class VolunteerDonation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CharityNeed")]
        public int? CharityNeedId { get; set; }
        public CharityNeed? CharityNeed { get; set; }
        public string VolunteerName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public decimal AmountDonated { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
