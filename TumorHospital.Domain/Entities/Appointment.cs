using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Principal;

namespace TumorHospital.Domain.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }


//    status VARCHAR(50) DEFAULT 'Scheduled',
}
