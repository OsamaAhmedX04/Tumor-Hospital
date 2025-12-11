namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorWorkDayDto
    {
        public string Day { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
