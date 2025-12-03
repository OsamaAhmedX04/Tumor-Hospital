namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorDetailsDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string? Bio { get; set; }
        public string Specialization { get; set; } 
        public List<DoctorWorkDayDto> WorkingDays { get; set; } = new List<DoctorWorkDayDto>();
    }
}
