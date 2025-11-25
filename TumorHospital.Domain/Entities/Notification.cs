using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Description { get; set; }
        public string RouteAction { get; set; }
        public DateTime? SentAt { get; set; }
        public bool IsSeen { get; set; }
    }
}
