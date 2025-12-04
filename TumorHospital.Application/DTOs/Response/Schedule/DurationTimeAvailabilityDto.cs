namespace TumorHospital.Application.DTOs.Response.Schedule
{
    public class DurationTimeAvailabilityDto
    {
        public TimeSpan FromTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
