using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Response.Pharmacy
{
    public class PharmacistDto
    {
        public Guid PharmacistId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalSales { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HireDate { get; set; }
    }
}
