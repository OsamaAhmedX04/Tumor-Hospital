using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Admin
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsSuperAdmin { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
