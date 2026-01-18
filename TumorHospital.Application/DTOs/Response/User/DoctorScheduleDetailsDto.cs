namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorScheduleDetailsDto
    {
        public string doctorId { get; set; }
        public string doctorName { get; set; }
        public List<ScheduleDetailsDto> scheduleDetails { get; set; }
    }
}
