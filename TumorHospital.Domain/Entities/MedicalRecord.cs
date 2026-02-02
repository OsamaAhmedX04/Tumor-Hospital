using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Patient")]
        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string RecordType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }


        public ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();
    }

}
