namespace TumorHospital.Application.DTOs.Response.User
{
    public class SpecializationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
