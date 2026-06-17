using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Medicine
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("CreatedByPharmacist")]
        public string? CreatedByPharmacistId { get; set; }
        public Pharmacist? CreatedByPharmacist { get; set; }

        [ForeignKey("Supplier")]
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public ICollection<MedicinePurchaseOrder> purchaseOrders { get; set; } = new List<MedicinePurchaseOrder>();
        public ICollection<MedicineSale> MedicineSales { get; set; } = new List<MedicineSale>();
    }
}
