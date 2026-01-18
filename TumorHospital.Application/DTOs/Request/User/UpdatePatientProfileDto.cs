namespace TumorHospital.Application.DTOs.Request.User
{
    public class UpdatePatientProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }

}
