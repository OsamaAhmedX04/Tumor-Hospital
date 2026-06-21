namespace TumorHospital.Application.DTOs.Response.Pharmacy
{
    public class PharmacistDto
    {
        public string PharmacistId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalSales { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HireDate { get; set; }
    }
}
