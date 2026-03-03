using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class VideoCall
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public string CallerId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        public string ReceiverId { get; set; }
        public Doctor? Doctor { get; set; }

        [ForeignKey("Appointment")]
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public bool IsActive { get; set; }

        public CallStatus Status { get; set; }
    }
}
