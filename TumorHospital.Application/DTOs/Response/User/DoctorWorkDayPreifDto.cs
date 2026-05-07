namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorWorkDayPreifDto
    {
        public Guid ScheduleId { get; set; }
        public string Day { get; set; } = null!;
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
