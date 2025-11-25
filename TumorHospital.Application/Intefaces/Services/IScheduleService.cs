using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IScheduleService
    {
        Task AddSchedule(string doctorId, DoctorScheduleDto doctorSchedule);
        Task DeleteScheduale(Guid scheduleId);
        Task UpdateScheduale(Guid scheduleId, string doctorId, DoctorScheduleDto schedule);
    }
}
