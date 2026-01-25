using Microsoft.EntityFrameworkCore;
using TumorHospital.Application.Intefaces.BackgroundServices;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.BackgroundJobs.BackgroundServices
{
    public class BackgroundAppointment : IBackgroundAppointment
    {
        private readonly IUnitOfWork _unitOfWork;

        public BackgroundAppointment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SetApprovedAppointmentsStatusToAbsent()
        {
            DayOfWeek today = DateTime.Now.DayOfWeek;

            // Previous day
            DayOfWeek previousDay = (DayOfWeek)(((int)today + 6) % 7);

            Day prevDay = previousDay switch
            {
                DayOfWeek.Saturday => Day.Saturday,
                DayOfWeek.Sunday => Day.Sunday,
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                DayOfWeek.Friday => Day.Friday,
                _ => Day.Friday
            };

            var appointments = _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .Where(a => a.Status == AppointmentStatus.Approved && a.DayOfWeek == prevDay);

            if (appointments is not null)
            {
                await appointments.ExecuteUpdateAsync(setter => setter.SetProperty(a => a.Status, AppointmentStatus.Absent));

                // canceling bill
                var appointmentsId = await appointments.Select(a => a.Id).ToListAsync();
                await _unitOfWork.Bills
                    .GetAllAsIQueryable()
                    .Where(b => appointmentsId.Contains(b.AppointmentId!.Value))
                    .ExecuteUpdateAsync(setter => setter.SetProperty(b => b.Status, BillStatus.Cancelled));
            }
        }
    }
}
