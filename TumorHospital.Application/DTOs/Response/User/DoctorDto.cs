namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; } = null!;
    }
}
