using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Domain.Entities
{
    public class Pharmacist
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Pharmacy")]
        public Guid? PharmacyId { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        public DateTime HireDate { get; set; }

        public ICollection<Medicine> MedicinesCreated { get; set; } = new List<Medicine>();
        public ICollection<MedicinePurchaseOrder> purchaseOrders { get; set; } = new List<MedicinePurchaseOrder>();
        public ICollection<MedicineSale> MedicineSales { get; set; } = new List<MedicineSale>();

    }
}
