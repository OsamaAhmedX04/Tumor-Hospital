using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.Domain.Entities
{
    public class Doctor
    {
        [Key]
        [ForeignKey("User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Hospital")]
        public Guid? HospitalId { get; set; }
        public Hospital? Hospital { get; set; }

        public string Gender { get; set; }

        [ForeignKey("Specialization")]
        public Guid? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public bool IsSurgeon { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? Bio { get; set; }
        public decimal ConsultationCost { get; set; }
        public decimal FollowUpCost { get; set; }
        public decimal? SurgeryCost { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }



}
