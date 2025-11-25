using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(40)]
        public string FirstName { get; set; }

        [Required, MaxLength(40)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public RefreshTokenAuth RefreshTokenAuth { get; set; }

        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<Receptionist> Receptionists { get; set; } = new List<Receptionist>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
