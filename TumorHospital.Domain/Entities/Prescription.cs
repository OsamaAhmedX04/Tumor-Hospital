using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Prescription
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


//    medication NVARCHAR(MAX) NOT NULL,
//    dosage NVARCHAR(MAX),

}
