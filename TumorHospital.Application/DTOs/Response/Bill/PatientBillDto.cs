using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.DTOs.Response.Bill
{
    public class PatientBillDto
    {
        public Guid BillId { get; set; }
        public string PatientName { get; set; }
        public string BillCode { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public BillStatus Status { get; set; }
    }
}
