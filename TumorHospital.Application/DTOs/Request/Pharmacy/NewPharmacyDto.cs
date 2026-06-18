using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Request.Pharmacy
{
    public class NewPharmacyDto
    {
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
