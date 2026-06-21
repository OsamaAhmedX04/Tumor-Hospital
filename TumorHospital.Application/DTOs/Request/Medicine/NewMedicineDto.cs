namespace TumorHospital.Application.DTOs.Request.Medicine
{
    public class NewMedicineDto
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
    }
}
