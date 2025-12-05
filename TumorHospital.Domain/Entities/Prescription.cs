using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Prescription
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Appointment")]
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
