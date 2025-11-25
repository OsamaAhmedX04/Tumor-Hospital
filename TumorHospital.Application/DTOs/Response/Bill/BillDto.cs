using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.DTOs.Response.Bill
{
    public class BillDto
    {
        public Guid BillId { get; set; }
        public string PatientName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public BillStatus Status { get; set; }
    }
}
