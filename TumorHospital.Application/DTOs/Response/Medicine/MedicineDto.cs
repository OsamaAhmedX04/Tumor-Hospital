namespace TumorHospital.Application.DTOs.Response.Medicine
{
    public class MedicineDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
        public Guid? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
