using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.Schedule;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddSchedule(string doctorId, DoctorScheduleDto doctorSchedule)
        {
            var doctorSchedules = await _unitOfWork.DoctorSchedules
                .GetAllAsync(
                    filter: ds => ds.DoctorId == doctorId,
                    selector: ds => new
                    {
                        DayOfWeek = ds.DayOfWeek,
                    }
                );
            bool isDuplicationExistDay =
                doctorSchedules.Any(ds => ds.DayOfWeek.ToString() == doctorSchedule.DayOfWeek);
            if (isDuplicationExistDay)
                throw new Exception("This Doctor Already Work At this day");

            if (doctorSchedules.Count == 5)
                throw new Exception("Maximun Days of Work for a doctor is 5 Days");

            var dayOfWeek = doctorSchedule.DayOfWeek switch
            {
                "Saturday" => Day.Saturday,
                "Sunday" => Day.Sunday,
                "Monday" => Day.Monday,
                "Tuesday" => Day.Tuesday,
                "Wednesday" => Day.Wednesday,
                "Thursday" => Day.Thursday,
                "Friday" => Day.Friday,
                _ => Day.Saturday
            };
            var newSchedule = new DoctorSchedule
            {
                DayOfWeek = dayOfWeek,
                StartTime = doctorSchedule.StartTime,
                DoctorId = doctorId,
                EndTime = doctorSchedule.StartTime.Add(TimeSpan.FromHours(8)),
                IsAvailable = true
            };
            await _unitOfWork.DoctorSchedules.AddAsync(newSchedule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteScheduale(Guid scheduleId)
        {
            var numberOfDoctorWorkDays = await _unitOfWork.DoctorSchedules
                .GetAllAsIQueryable()
                .GroupBy(ds => ds.DoctorId)
                .Select(g => g.Count())
                .FirstOrDefaultAsync();
            if (numberOfDoctorWorkDays <= 3)
                throw new Exception("Each Doctor Must have at least 3 days of work");

            _unitOfWork.DoctorSchedules.Delete(scheduleId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> IsWorkIn(string doctorId, string day)
        {
            if(!Enum.TryParse<Day>(day, out _))
                throw new Exception("Invalid Day Provided");
            
                var dayOfWeek = day switch
                {
                    "Saturday" => Day.Saturday,
                    "Sunday" => Day.Sunday,
                    "Monday" => Day.Monday,
                    "Tuesday" => Day.Tuesday,
                    "Wednesday" => Day.Wednesday,
                    "Thursday" => Day.Thursday,
                    "Friday" => Day.Friday,
                    _ => Day.Saturday
                };
            var isWorkIn = await _unitOfWork.DoctorSchedules.AnyAsync(ds =>
                ds.DoctorId == doctorId && ds.DayOfWeek == dayOfWeek);

            return isWorkIn;
        }

        public async Task UpdateScheduale(Guid scheduleId,string doctorId, DoctorScheduleDto schedule)
        {
            var isThereDuplicationDay = await IsDuplicatedDoctorWorkDayAsync(doctorId, schedule.DayOfWeek);
            if (isThereDuplicationDay)
                throw new Exception("This Doctor Already Work On This Day");

            var dayOfWeek = schedule.DayOfWeek switch
            {
                "Saturday" => Day.Saturday,
                "Sunday" => Day.Sunday,
                "Monday" => Day.Monday,
                "Tuesday" => Day.Tuesday,
                "Wednesday" => Day.Wednesday,
                "Thursday" => Day.Thursday,
                "Friday" => Day.Friday,
                _ => Day.Saturday
            };
            await _unitOfWork.DoctorSchedules.GetAllAsIQueryable()
                .Where(ds => ds.Id == scheduleId)
                .ExecuteUpdateAsync(ds => ds
                    .SetProperty(ds => ds.StartTime, schedule.StartTime)
                    .SetProperty(ds => ds.DayOfWeek, dayOfWeek)
                    .SetProperty(ds => ds.LastModified, DateTime.Now)
                    );
        }

        public async Task<List<DurationTimeAvailabilityDto>> GetAvailableTimes(string doctorId, string day)
        {
            if (!await IsWorkIn(doctorId, day))
                throw new Exception("Doctor Not Work In That Day");
            var dayOfWeek = day switch
            {
                "Saturday" => Day.Saturday,
                "Sunday" => Day.Sunday,
                "Monday" => Day.Monday,
                "Tuesday" => Day.Tuesday,
                "Wednesday" => Day.Wednesday,
                "Friday" => Day.Friday,
                _ => throw new Exception("Invalid Day")
            };
            var durationTime = await _unitOfWork.DoctorSchedules.GetEnhancedAsync(
                filter: ds => ds.DoctorId == doctorId && ds.DayOfWeek == dayOfWeek,
                selector: ds => new DurationTimeDto
                {
                    FromTime = ds.StartTime,
                    ToTime = ds.EndTime
                }
                );
            var appointmentsTimes = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.Status == AppointmentStatus.Approved && a.DayOfWeek == dayOfWeek,
                selector: a => new DurationTimeDto
                {
                    FromTime = a.FromTime!.Value,
                    ToTime = a.ToTime!.Value,
                }
                );

            List<DurationTimeAvailabilityDto> availableTimes = GetAvailableTimesDuration(durationTime!,appointmentsTimes);
            
            return availableTimes;
        }

        private List<DurationTimeAvailabilityDto> GetAvailableTimesDuration(DurationTimeDto durationTime, List<DurationTimeDto> appointmentsTimes)
        {
            var startTime = durationTime.FromTime;
            var times = new List<TimeSpan>();
            while(startTime != durationTime.ToTime)
            {
                times.Add(startTime);
                startTime = startTime.Add(TimeSpan.FromMinutes(30));
            }

            List<DurationTimeAvailabilityDto> availableTimes = new List<DurationTimeAvailabilityDto>();
            foreach(var time in times)
            {
                if (appointmentsTimes.Select(at => at.FromTime).Contains(time))
                    availableTimes.Add(new DurationTimeAvailabilityDto { FromTime = time, IsAvailable = false });
                else
                    availableTimes.Add(new DurationTimeAvailabilityDto { FromTime = time, IsAvailable = true });
            }
            return availableTimes;
        }

        private async Task<bool> IsDuplicatedDoctorWorkDayAsync(string doctorId, string dayOfWeek)
        {
            var day = dayOfWeek switch
            {
                "Saturday" => Day.Saturday,
                "Sunday" => Day.Sunday,
                "Monday" => Day.Monday,
                "Tuesday" => Day.Tuesday,
                "Wednesday" => Day.Wednesday,
                "Thursday" => Day.Thursday,
                "Friday" => Day.Friday,
                _ => Day.Saturday
            };
            var doctor = await _unitOfWork.Doctors
                .GetAsync(
                filter: d => d.ApplicationUserId == doctorId,
                selector: d => new
                {
                    Schedules = d.Schedules,
                },
                Includes: d => d.Schedules
                );

            return doctor.Schedules.Any(s => s.DayOfWeek == day);
        }

        
    }
}
