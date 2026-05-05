namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorProfileResponse
    {
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? SpecializationName { get; set; }
    }


}
