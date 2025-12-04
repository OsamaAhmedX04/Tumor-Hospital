namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class NewSurgeryAppointmentDto
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string DayOfWeek { get; set; }
    }
}
