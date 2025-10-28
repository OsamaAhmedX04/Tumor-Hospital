using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Receptionist
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
