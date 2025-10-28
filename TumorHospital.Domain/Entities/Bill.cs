using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public decimal TotalAmount { get; set; }
        public BillStatus Status { get; set; }

        [ForeignKey("Receptionist")]
        public string? ConfirmedBy { get; set; }
        public Receptionist? Receptionist { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }


//    insurance_claim DECIMAL(10,2) DEFAULT 0,
//    amount_due DECIMAL(10,2) NOT NULL,

}
