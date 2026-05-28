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
            var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

            var appointments = _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .Where(a =>
                    a.Status == AppointmentStatus.Approved &&
                    a.AttendenceDate!.Value == yesterday);

            var appointmentsId = await appointments
                .Select(a => a.Id)
                .ToListAsync();

            if (!appointmentsId.Any())
                return;

            await appointments.ExecuteUpdateAsync(setter =>
                setter.SetProperty(a => a.Status, AppointmentStatus.Absent));

            await _unitOfWork.Bills
                .GetAllAsIQueryable()
                .Where(b => appointmentsId.Contains(b.AppointmentId!.Value))
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(b => b.Status, BillStatus.Cancelled));
        }
    }
}
