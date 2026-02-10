using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Patient
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateOnly? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }


        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<MentalHealthSurvey> MentalHealthSurvies { get; set; } = new List<MentalHealthSurvey>();
        public ICollection<VideoCall> VideoCalls { get; set; } = new List<VideoCall>();

    }

}
