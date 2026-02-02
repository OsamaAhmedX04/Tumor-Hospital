using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class AboutInfo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string HospitalName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Mission { get; set; }

        [Required]
        public string Vision { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
