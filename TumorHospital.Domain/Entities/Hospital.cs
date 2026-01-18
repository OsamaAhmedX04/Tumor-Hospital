using System.ComponentModel.DataAnnotations;

namespace TumorHospital.Domain.Entities
{
    public class Hospital
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Government { get; set; }
        public string Address { get; set; }
        public int MaxNumberOfDoctors { get; set; }
        public int MaxNumberOfReceptionists { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public ICollection<Receptionist> Receptionists { get; set; } = new List<Receptionist>();
    }
}
