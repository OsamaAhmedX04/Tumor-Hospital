namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class NewConsultationAppointmentDto
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string DayOfWeek { get; set; }
    }
}
