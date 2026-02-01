using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class Bill
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Appointment")]
        public Guid? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }

        [ForeignKey("Offer")]
        public Guid? AppliedOfferId { get; set; }
        public Offer Offer { get; set; }
        public decimal? AppliedOfferPercentage { get; set; }
        public string Code { get; set; }
        public BillStatus Status { get; set; }

        [ForeignKey("Receptionist")]
        public string? ConfirmedBy { get; set; }
        public Receptionist? Receptionist { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
