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
        public string Specialization { get; set; }
        public string ProfilePicturePath { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }


//    certifications NVARCHAR(MAX),

}
