using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Diagnostic
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MedicalRecord")]
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public string ModelOutput { get; set; }
        public string TumorLocation { get; set; }
        public decimal ConfidenceScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
