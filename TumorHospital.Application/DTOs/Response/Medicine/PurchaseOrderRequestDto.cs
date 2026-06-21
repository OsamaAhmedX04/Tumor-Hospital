namespace TumorHospital.Application.DTOs.Response.Medicine
{
    public class PurchaseOrderRequestDto
    {
        public string MedicineName { get; set; } = null!;
        public string SupplierName { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string CreatedByPharmacistName { get; set; }
        public string CreatedByPharmacistPhoneNumber { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyLocation { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
