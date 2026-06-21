using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class Supplier
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
        public ICollection<MedicinePurchaseOrder> purchaseOrders { get; set; } = new List<MedicinePurchaseOrder>();
    }
}
