using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Domain.Entities
{
    public class CharityNeed
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public CharityCategory Category { get; set; }
        public decimal NeedAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<VolunteerDonation> VolunteerDonations { get; set; } = new List<VolunteerDonation>();
    }
}
