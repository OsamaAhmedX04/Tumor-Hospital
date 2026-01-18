namespace TumorHospital.Application.DTOs.Response.User
{
    public class ReceptionistDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
