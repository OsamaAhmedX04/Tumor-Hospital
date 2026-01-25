namespace TumorHospital.Application.DTOs.Response.Bill
{
    public class PatientBillDto
    {
        public Guid BillId { get; set; }
        public string PatientName { get; set; }
        public string BillCode { get; set; }
        public decimal TotalAmount { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
