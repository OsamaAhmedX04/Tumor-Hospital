namespace TumorHospital.Application.DTOs.Response.Appointment
{
    public class AppointmentDurationDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AppointmentReason { get; set; }
        public string PatientName { get; set; }
    }
}
