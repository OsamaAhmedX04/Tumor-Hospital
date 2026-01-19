using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorInformationDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string? Bio { get; set; }
        public string Specialization { get; set; }
        public bool IsSurgeon { get; set; }
        public decimal ConsultationCost { get; set; }
        public decimal FollowUpCost { get; set; }
        public decimal? SurgeryCost { get; set; }
        public List<DoctorWorkDayPreifDto> WorkingDays { get; set; } = new List<DoctorWorkDayPreifDto>();
    }
}
