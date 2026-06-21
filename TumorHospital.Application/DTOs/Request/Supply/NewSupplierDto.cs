namespace TumorHospital.Application.DTOs.Request.Supply
{
    public class NewSupplierDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhone { get; set; }
    }
}
