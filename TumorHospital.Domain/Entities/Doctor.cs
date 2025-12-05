using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace TumorHospital.Domain.Entities
{
    public class Doctor
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Gender { get; set; }
        
        [ForeignKey("Specialization")]
        public Guid? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? Bio { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }



}
