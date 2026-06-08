using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Diagnostic
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Appointment")]
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string ImageURL { get; set; }

        public string PredictedClass { get; set; }
        public decimal ConfidenceScore { get; set; }
        public decimal GliomaProbability { get; set; }
        public decimal MeningiomaProbability { get; set; }
        public decimal NoTumorProbability { get; set; }
        public decimal PituitaryProbability { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
