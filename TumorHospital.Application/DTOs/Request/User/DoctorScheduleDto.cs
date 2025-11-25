namespace TumorHospital.Application.DTOs.Request.User
{
    public class DoctorScheduleDto
    {
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
