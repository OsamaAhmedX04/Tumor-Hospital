using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class DoctorSchedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Day DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }

    }
}
