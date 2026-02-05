using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class Offer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal DiscountPercentage { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
