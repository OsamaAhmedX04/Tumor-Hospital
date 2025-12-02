using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace TumorHospital.Application.DTOs.Request.User
{
    public class NewDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string SpecializationName { get; set; }

        public List<DoctorScheduleDto> Schedules { get; set; } = new List<DoctorScheduleDto>();
    }
}
