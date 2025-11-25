namespace TumorHospital.Application.DTOs.Response.User
{
    public class ScheduleDetailsDto
    {
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
