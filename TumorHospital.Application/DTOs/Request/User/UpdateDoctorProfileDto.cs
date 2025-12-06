using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.DTOs.Request.User
{
    public class UpdateDoctorProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string? Bio { get; set; }
    }

}
