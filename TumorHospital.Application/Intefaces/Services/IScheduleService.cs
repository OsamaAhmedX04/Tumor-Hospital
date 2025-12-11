using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.Schedule;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IScheduleService
    {
        Task<List<DoctorWorkScheduleDto>> GetDoctorSchedule(string doctorId);
        Task AddSchedule(string doctorId, DoctorScheduleDto doctorSchedule);
        Task DeleteScheduale(Guid scheduleId);
        Task UpdateScheduale(Guid scheduleId, string doctorId, DoctorScheduleDto schedule);
        Task<bool> IsWorkIn(string doctorId, string day);
        Task<List<DurationTimeAvailabilityDto>> GetAvailableTimes(string doctorId, string day);
    }
}
