using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Principal;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public AppointmentReason Reason { get; set; }
        public Day DayOfWeek { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime RequestCreatedAt { get; set; }
        public DateOnly? AttendenceDate { get; set; }
    }
}
