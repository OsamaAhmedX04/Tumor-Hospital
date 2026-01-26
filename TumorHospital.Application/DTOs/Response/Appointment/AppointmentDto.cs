using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Application.DTOs.Response.Appointment
{
    public class AppointmentDto
    {
        [Key]
        public Guid AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string? DoctorName { get; set; }
        public string? DoctorImagePath { get; set; }
        public string Reason { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Status { get; set; }
        public bool IsPrescriptionExist { get; set; }
        public DateTime RequestCreatedAt { get; set; }
        public DateOnly? AttendenceDate { get; set; }
    }
}
