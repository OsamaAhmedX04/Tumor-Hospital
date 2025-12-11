using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Response.Appointment;

namespace TumorHospital.Application.DTOs.Response.Schedule
{
    public class DoctorWorkScheduleDto
    {
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<AppointmentDurationDto> appointmentDurations { get; set; } = new List<AppointmentDurationDto>();
    }
}
