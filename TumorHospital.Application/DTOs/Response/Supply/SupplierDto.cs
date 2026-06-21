namespace TumorHospital.Application.DTOs.Response.Supply
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
