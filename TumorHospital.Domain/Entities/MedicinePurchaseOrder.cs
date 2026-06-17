using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class MedicinePurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Medicine")]
        public Guid? MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        [ForeignKey("Supplier")]
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [ForeignKey("Pharmacy")]
        public Guid? PharmacyId { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        [ForeignKey("CreatedByPharmacist")]
        public string? CreatedByPharmacistId { get; set; }
        public Pharmacist? CreatedByPharmacist { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
