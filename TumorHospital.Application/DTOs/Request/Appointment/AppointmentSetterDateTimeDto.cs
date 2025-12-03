namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class AppointmentSetterDateTimeDto
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public DateTime AttendenceDate { get; set; }
    }
}
