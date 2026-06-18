using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.DTOs.Request.Medicine
{
    public class NewMedicineDto
    {
        public string CreatedByPharmacistId { get; set; } = null!;
        public Guid SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
    }
}
