namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorProfileResponse
    {
        public string ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? SpecializationName { get; set; }
    }


}
