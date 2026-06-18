using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.DTOs.Response.Pharmacy
{
    public class PharmacyDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int NumberOfPharmacist { get; set; }
        public decimal TotlaProfit { get; set; }
        public List<PharmacistDto> pharmacists { get; set; } = new List<PharmacistDto>();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
