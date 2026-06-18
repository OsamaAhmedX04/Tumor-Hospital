using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Request.Supply
{
    public class UpdateSupplierDto
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhone { get; set; }
    }
}
