namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorDto
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
